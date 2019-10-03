namespace VendM.Core
{
    public class VideoConverToImg
    {
        /// <summary>
        /// 从视频中截取img格式图片
        /// </summary>
        /// <param name="applicationPath">ffmpeg.exe文件路径</param>
        /// <param name="fileNamePath">视频文件路径（带文件名）</param>
        /// <param name="targetImgNamePath">生成img图片路径（带文件名）</param>
        public static void ConverToImg(string applicationPath, string fileNamePath, string targetImgNamePath)
        {

            //string c = applicationPath + @"\ffmpeg.exe -i" + fileNamePath + targetImgNamePath+"-ss 00:00:05  -r 1 -vframes 1 -an -vcodec mjpeg " ;
            string c = applicationPath + @"\ffmpeg.exe -ss 00:00:05 -i" + " " + fileNamePath + " " + targetImgNamePath + " " + "-r 1 -vframes 1 -an -vcodec mjpeg ";//速度快
            Cmd(c);
            //-i:设定输入文件名
            //-r：设定帧 此处设为1帧
            //-f:设定输出格式
            //-ss 从指定时间截图
            //-vcodec：设定影像解码器，没有输入时为文件原来相同的解码器
            //-vframes 设置转换多少桢(frame)的视频
            //-an 不处理声音
        }

        /// <summary>
        /// 程序中调用CMD.exe程序，并且不显示命令行窗口界面
        /// </summary>
        /// <param name="c">执行的cmd命令</param>
        private static void Cmd(string c)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.CreateNoWindow = true; //不显示程序窗口                
                process.StartInfo.FileName = "cmd.exe";//要执行的程序名称 
                process.StartInfo.UseShellExecute = false;
                //process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息 
                process.StartInfo.RedirectStandardInput = true; //可能接受来自调用程序的输入信息 
                process.Start();//启动程序 
                process.StandardInput.WriteLine(c);
                process.StandardInput.AutoFlush = true;
                process.StandardInput.WriteLine("exit");

            }
            catch { }
        }
    }
}
