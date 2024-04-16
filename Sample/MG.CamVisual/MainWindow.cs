using MG.CamCtrl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace MG.CamVisual
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();

            InitUI();
        }

        #region  相机操作
        private ICamera myCamera { get; set; }

        public bool isTriggerModel = false;

        public bool isConnected = false;


        private void Btn_SearchCam_Click(object sender, EventArgs e)
        {
            var cambrand = (CameraBrand)Enum.Parse(typeof(CameraBrand), BrandCombox.SelectedItem.ToString());

            List<string> listsn = CamFactory.GetDeviceEnum(cambrand);

            if (listsn.Count > 0)
            {
                SNCombox.Items.Clear();
                SNCombox.Items.AddRange(listsn.ToArray());
                SNCombox.SelectedIndex = 0;
                Log($"设备枚举成功");
            }
            else
            {
                Log("未查询到设备！");
                MessageBox.Show("未查询到设备！", "提示", MessageBoxButtons.OK);
                SNCombox.Items.Clear();
            }
        }
         
        private void Btn_CamInit_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                myCamera.CloseDevice();
                myCamera = null; 
            }
            var brand = (CameraBrand)Enum.Parse(typeof(CameraBrand), BrandCombox.SelectedItem.ToString());

            myCamera = CamFactory.CreatCamera(brand);
            bool res = myCamera.InitDevice(SNCombox.SelectedItem.ToString());

            if (!res)
            {
                Log($"[{SNCombox.SelectedItem}]初始化失败！");
                MessageBox.Show($"[{SNCombox.SelectedItem}]初始化失败！", "提示", MessageBoxButtons.OK);
                myCamera.CloseDevice();
                myCamera = null;
                isConnected = false;
            }
            Log($"[{SNCombox.SelectedItem}]相机初始化");
            GetCamconfig();
            isConnected = true;
            UpdateUI(isConnected, false); 
            EnableParm(true);
            DisplayBox.Image = null;

        }


        private void Btn_Startup_Click(object sender, EventArgs e)
        { 
            if (isTriggerModel)
            { //触发模式

                if (Rbtn_Trigger_Soft.Checked)
                { //软触发
                    isConnected = myCamera.StartWith_SoftTriggerModel();
                    Log($"软触发模 式启动");
                }
                else if (Rbtn_Trigger_SoftCallback.Checked)
                { //软触发+回调
                    Log($"软触发 + 回调模式 启动");
                    isConnected = myCamera.StartWith_SoftTriggerModel_SetCallback(CamCallBack);
                }
                else if (Rbtn_Trigger_Hard.Checked)
                { //硬触发
                    Log($"硬触发模式 启动");
                    TriggerSource linesource = (TriggerSource)Enum.Parse(typeof(TriggerSource), Combobox_HardSource.SelectedItem.ToString());
                    isConnected = myCamera.StartWith_HardTriggerModel(linesource);
                }
                else if (Rbtn_Trigger_HardCallback.Checked)
                { //硬触发 + 回调
                    Log($"硬触发 + 回调模式 启动");
                    TriggerSource linesource = (TriggerSource)Enum.Parse(typeof(TriggerSource), Combobox_HardSource.SelectedItem.ToString());
                    isConnected = myCamera.StartWith_HardTriggerModel_SetCallback(linesource, CamCallBack);
                }
            }
            else
            { //连续触发
                Log($"连续 + 回调模式 启动");
                isConnected = myCamera.StartWith_Continue_SetCallback(CamCallBack);
            }

            //更新UI
            UpdateUI(isConnected,true); 

        }

        private void Btn_Destroy_Click(object sender, EventArgs e)
        {
            myCamera?.CloseDevice();
            myCamera = null;
            isConnected = false;

            //更新UI
            UpdateUI(isConnected,false);
            EnableParm(false);
            DisplayBox.Image = null;

            Log($"[{SNCombox.SelectedItem}] 相机注销");
        }

        private void Rbtn_ModelChanged(object sender, EventArgs e)
        {
            if (Rbtn_ContinueModel.Checked)
            {//选择连续模式
                isTriggerModel = false;
                EnableTriggerItem(false);
                Log($"切换为连续模式");
            }
            else if (Rbtn_TriggerModel.Checked)
            {//选择触发模式 
                isTriggerModel = true;
                EnableTriggerItem(true);
                Rbtn_Trigger_Soft.Checked = true;
                Log($"切换为触发模式");
            }
        }

        private void Rbtn_TriggerStyleChanged(object sender, EventArgs e)
        {
            Btn_SoftTrigger.Enabled = Rbtn_Trigger_Soft.Checked;
            Btn_SoftTrigger_Callback.Enabled = Rbtn_Trigger_SoftCallback.Checked;
            Combobox_HardSource.Enabled = Rbtn_Trigger_Hard.Checked;
            Combobox_HardSource_Callback.Enabled = Rbtn_Trigger_HardCallback.Checked;

        }

        private void Btn_SoftTrigger_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                Log($"软触发取图");
                myCamera.GetImageWithSoftTrigger(out Bitmap res, 5000);
                DisplayBox.Image = res.Clone() as Bitmap;
                res?.Dispose();
                
            }
        }

        private void Btn_SoftTrigger_Callback_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                Log($"软触发");
                myCamera?.SoftTrigger();
            }
        }

        /// <summary>
        /// 回调函数
        /// </summary>
        /// <param name="img"></param>
        private void CamCallBack(Bitmap img)
        {
            this.Invoke(new Action(() =>
            {
                if(isConnected)
                {
                    DisplayBox.Image = img.Clone() as Bitmap;
                    img?.Dispose();
                } 
            }));
        }

        #endregion

        #region 获取参数

        private void GetCamconfig()
        {
            CamConfig camcfg=null;
            if (myCamera != null)
            {
                myCamera?.GetCamConfig(out camcfg);
                Tbox_ExpouseTime.Text = camcfg.ExpouseTime.ToString();
                Tbox_Gain.Text = camcfg.Gain.ToString();
                Tbox_TriggerFliter.Text = camcfg.TriggerFilter.ToString();
                string dav = camcfg.triggerPolarity.ToString();
                cmbox_TriggerPolarity.SelectedIndex = cmbox_TriggerPolarity.Items.IndexOf( dav);
            }
        }

        #endregion
        #region 参数设置
        private void Tbox_ExpouseTime_TextChanged(object sender, EventArgs e)
        {
            bool res = ushort.TryParse(Tbox_ExpouseTime.Text, out ushort value);
            if (!res) return;
            if (isConnected && myCamera != null)
            {
                myCamera?.SetExpouseTime(value);
                Log($"设置曝光时长[{value}]");
            }
        }

        private void Tbox_Gain_TextChanged(object sender, EventArgs e)
        {
            bool res = short.TryParse(Tbox_Gain.Text, out short value);
            if (!res) return;
            if (isConnected && myCamera != null)
            {
                myCamera?.SetGain(value);
                Log($"设置增益[{value}]");
            }
        }

        private void Btn_AutoWhiteBlance_Click(object sender, EventArgs e)
        {
            if (isConnected && myCamera != null)
            {
                myCamera?.AutoBalanceWhite();
                Log($"自动白平衡");
            }
        }

        private void Tbox_TriggerFliter_TextChanged(object sender, EventArgs e)
        {
            bool res = ushort.TryParse(Tbox_TriggerFliter.Text, out ushort value);
            if (!res) return;
            if (isConnected && myCamera != null)
            {
                myCamera?.SetTriggerFliter(value);
                Log($"设置触发滤波[{value}]");
            }
        }
        #endregion

        #region 读写图片
        private void Btn_ReadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "图片(*.bmp)|*.bmp;*.png;*.jpg;*.jpeg| (*.png)|*.png;*.PNG| (*.jpg)|*.jpg;*.JPG| (*.jpeg)|*.jpeg;*.JPEG";
            openFileDialog.Multiselect = false;
            openFileDialog.CheckFileExists = true;
            openFileDialog.AddExtension = true;
            openFileDialog.RestoreDirectory = true;
            ;

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                if (!File.Exists(openFileDialog.FileName)) return;
                DisplayBox.Image = new Bitmap(openFileDialog.FileName);
            }
        }

        private void Btn_SaveImage_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "图片(*.bmp)|*.bmp;*.png;*.jpg;*.jpeg| (*.png)|*.png;*.PNG| (*.jpg)|*.jpg;*.JPG| (*.jpeg)|*.jpeg;*.JPEG";
                saveFileDialog.CheckFileExists = false;
                saveFileDialog.AddExtension = true;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff");

                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    DisplayBox.Image.Save(saveFileDialog.FileName);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 图片缩放

        private bool IsMove = false;
        private int ZoomStep = 50;
        private Point MouseDownPoint;

        private void DisplayBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (DisplayBox.Image == null) return;

            PictureBox pbox = DisplayBox;
            int x = e.Location.X;
            int y = e.Location.Y;
            int ow = pbox.Width;
            int oh = pbox.Height;
            int VX, VY;  //因缩放产生的位移矢量
            if (e.Delta > 0) //放大
            {
                //第1步
                pbox.Width += ZoomStep;
                pbox.Height += ZoomStep;
                //第2步
                PropertyInfo pInfo = pbox.GetType().GetProperty("ImageRectangle", BindingFlags.Instance | BindingFlags.NonPublic);
                Rectangle rect = (Rectangle)pInfo.GetValue(pbox, null);
                //第3步
                pbox.Width = rect.Width;
                pbox.Height = rect.Height;

                //Console.WriteLine(string.Format("宽：{0}，高：{1}",pbox.Width,pbox.Height));
            }
            if (e.Delta < 0) //缩小
            {
                //防止一直缩成负值
                if (pbox.Width < 300)
                    return;

                pbox.Width -= ZoomStep;
                pbox.Height -= ZoomStep;
                PropertyInfo pInfo = pbox.GetType().GetProperty("ImageRectangle", BindingFlags.Instance |
                 BindingFlags.NonPublic);
                Rectangle rect = (Rectangle)pInfo.GetValue(pbox, null);
                pbox.Width = rect.Width;
                pbox.Height = rect.Height;
            }

            //第4步，求因缩放产生的位移，进行补偿，实现锚点缩放的效果
            VX = (int)((double)x * (ow - pbox.Width) / ow);
            VY = (int)((double)y * (oh - pbox.Height) / oh);
            pbox.Location = new Point(pbox.Location.X + VX, pbox.Location.Y + VY);


        }

        private void DisplayBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (DisplayBox.Image == null) return;
            DisplayBox.Focus(); //鼠标在picturebox上时才有焦点，此时可以缩放
            if (IsMove)
            {
                int x, y;   //新的DisplayBox.Location(x,y)
                int moveX, moveY; //X方向，Y方向移动大小。
                moveX = Cursor.Position.X - MouseDownPoint.X;
                moveY = Cursor.Position.Y - MouseDownPoint.Y;
                x = DisplayBox.Location.X + moveX;
                y = DisplayBox.Location.Y + moveY;
                DisplayBox.Location = new Point(x, y);
                MouseDownPoint.X = Cursor.Position.X;
                MouseDownPoint.Y = Cursor.Position.Y;
            }
        }

        private void DisplayBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (DisplayBox.Image == null) return;

            if (e.Button == MouseButtons.Left)
            {
                MouseDownPoint.X = Cursor.Position.X; //记录鼠标左键按下时位置
                MouseDownPoint.Y = Cursor.Position.Y;
                IsMove = true;
                DisplayBox.Focus(); //鼠标滚轮事件(缩放时)需要picturebox有焦点
            }
        }
        private void DisplayBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                IsMove = false;
            }
        }
        #endregion

        #region other
        private void InitUI()
        {
            DisplayBox.MouseWheel += DisplayBox_MouseWheel;

            Btn_Startup.Enabled = false;
            Btn_Destroy.Enabled = false;

            Combobox_HardSource.SelectedIndex = 0;
            Combobox_HardSource_Callback.SelectedIndex = 0;
            BrandCombox.SelectedIndex = 0;
            cmbox_TriggerPolarity.SelectedIndex = 0;


            EnableTriggerItem(isTriggerModel);
            EnableParm(false); 
        }

        private void UpdateUI(bool camisConnected,bool isstarted)
        {
            Btn_CamInit.Enabled = !camisConnected;
            
            Btn_Startup.Enabled = isstarted? false:isConnected;
            Btn_Destroy.Enabled = camisConnected;
           
            //EnableTriggerItem(!camisConnected);
        }

        private void EnableTriggerItem(bool flag)
        {
            Rbtn_Trigger_Soft.Enabled = flag;
            Rbtn_Trigger_Hard.Enabled = flag;
            Rbtn_Trigger_SoftCallback.Enabled = flag;
            Rbtn_Trigger_HardCallback.Enabled = flag;
            Btn_SoftTrigger.Enabled = flag;
            Btn_SoftTrigger_Callback.Enabled = flag;

            if (!flag)
            {
                Btn_SoftTrigger.Enabled = false;
                Btn_SoftTrigger_Callback.Enabled = false;
                Combobox_HardSource.Enabled = false;
                Combobox_HardSource_Callback.Enabled = false;
            }
        }

        private void EnableParm(bool flag)
        {
            Tbox_ExpouseTime.Enabled = flag;
            Tbox_Gain.Enabled = flag;
            Btn_AutoWhiteBlance.Enabled = flag;
            cmbox_TriggerPolarity.Enabled = flag;
            Tbox_TriggerFliter.Enabled = flag;
        }
         
        private void Log(string message)
        {
            this.Invoke(new Action(() =>
            {
                if (Tbox_Log.Lines.Count() > 500)
                {
                    Tbox_Log.Clear();
                }
               
                Tbox_Log.AppendText($"\r\n[{DateTime.Now.ToString("HH:mm:ss:ff")}]:{message}");
            }));

        }

        private void menubtn_about_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.Show();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                CamFactory.DestroyAll();
                isConnected = false;

            }));
        }
        #endregion

    }
}

