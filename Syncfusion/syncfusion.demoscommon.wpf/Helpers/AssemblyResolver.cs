using System.IO;
namespace syncfusion.demoscommon.wpf
{
    public static class AssemblyResolver
    {
    }
    /// <summary>
    /// Logs the errors in ErrorLog.txt
    /// </summary>
    public static class ErrorLogging
    {
        /// <summary>
        /// Method to Clear previous logs
        /// </summary>
        internal static void ClearPreviousLogs()
        {
            string errorPath = Directory.GetCurrentDirectory() + @"\Errorlog.txt";
            if (File.Exists(errorPath))
                File.Delete(errorPath);
#if DEBUG
            string bindingErrorPath = Directory.GetCurrentDirectory() + @"\BindingError.txt";
            string livedemosPath = Directory.GetCurrentDirectory() + @"\LiveDemos.txt";
            if (File.Exists(bindingErrorPath))
                File.Delete(bindingErrorPath);
            if (File.Exists(livedemosPath))
                File.Delete(livedemosPath);
#endif
        }
        /// <summary>
        /// Method to take care of error logging operations
        /// </summary>
        /// <param name="error"></param>
        public static void LogError(object error)
        {
            // Obtains the array of string from obtained error details.
            var errorDetails = error.ToString().Split(new[] { "@@" }, StringSplitOptions.None);

            // Obtains the sample type, product name and sample name from the errordetails.
            var productPath = errorDetails[0].ToString().Split(new[] { "\\" }, StringSplitOptions.None);
            string currentDir = Directory.GetCurrentDirectory();

            if (productPath.Length > 2)
            {
                string dir = currentDir + @"\ErrorLog\" + productPath[6];
                //string dir = currentDir + @"\ErrorLog\" + productPath[0] + @"\" + productPath[1];
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                dir += @"\" + productPath[2] + ".txt";
                if (!File.Exists(dir))
                {
                    File.Create(dir).Close();
                }

                // Checks and prevents writing same error for same sample type, product name and sample name.
                var reader = File.ReadAllLines(dir).ToList();
                if (reader.Count > 0)
                {
                    if (reader.Contains(errorDetails[1].ToString()))
                    {
                        return;
                    }
                }

                using (StreamWriter fileWriter = File.AppendText(dir))
                {
                    fileWriter.Write(errorDetails[0].ToString() + "\n");
                    //fileWriter.Write(errorDetails[1].ToString() + "\n");
                    fileWriter.Close();
                }
            }
        }
    }
}
