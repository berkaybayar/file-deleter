using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsAppText //ENTER YOUR PROJECT NAMESPACE HERE TO USE VARIABLES AND FUNCTIONS
{
    class FileDeleter
{

        public static string currentdir = Directory.GetCurrentDirectory();//gets current directory 
        public static string userdir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);//gets user directory C:\Users\%username%
        public static string desktopdir = userdir + "\\Desktop";//gets desktop directory C:\Users\%username%\desktop
        public static string appdatadir = userdir + "\\AppData\\Roaming";//gets appdata directory C:\Users\%username%\AppData\\Roaming
        public static string systemdir = Environment.GetFolderPath(Environment.SpecialFolder.System);//gets system32 directory C:\Windows\System32
        public static string logdir = currentdir + "\\logs\\"; //log file directory
        public static string path; //path of the file that will be deleted

        public static ArrayList faillog = new ArrayList();//to log deletion failed items
        public static ArrayList succlog = new ArrayList(); //to log deletion successful items


        public static int fitemcount = 1; //failed deletion item count 
        public static int sitemcount = 1; //success deletion item count

        public static int flogcount = 1; //failed deletion log count
        public static int slogcount = 1; //failed deletion log count

        public static bool flogenable = false; //to enable failed deletion logger change this to true
        public static bool slogenable = false; //to enable successful deletion logger change this to true



    void deleteallfiles() //deletes all files in a folder or deletes the file 
    {
        /*
        Before this action send the folder or the file you wanna delete to path string variable if you dont do this action will fail
        Example:
        path = userdir + "\\Desktop\\%FILENAME%"
        deleteallfiles();

        */
        DirectoryInfo di = new DirectoryInfo(path);
        //FileInfo fi = new FileInfo(path);
        try
        {
                
            foreach (FileInfo file in di.GetFiles())
            {
                try
                {
                    file.Delete();
                    if (slogenable == true)
                    {
                           
                        succlog.Add("Deletion successful: " + file + " " + DateTime.Now.ToShortTimeString());
                        sitemcount++;
                    }
                }
                catch
                {
                    if (flogenable == true)
                    {
                        faillog.Add("Deletion failed: " + file + " " + DateTime.Now.ToShortTimeString());
                        fitemcount++;
                    }

                    continue;
                }
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                try
                {
                    dir.Delete(true);
                    if (slogenable == true)
                    {
                        succlog.Add("Deletion successful: " + dir + " " + DateTime.Now.ToShortTimeString());
                        sitemcount++;
                    }
                }
                catch
                {
                    if (flogenable == true)
                    {
                        faillog.Add("Deletion failed: " + dir + " " + DateTime.Now.ToShortTimeString());
                        fitemcount++;
                    }
                    continue;
                }
            }
            path = ""; //clears the content of path string variable
        }
        catch 
        {
            try
            {
                File.Delete(path);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured: " + ex);
            }
            
        }
    }

    void successlogger() //logger for successful folders,files deletion
    {
        Directory.CreateDirectory(logdir);//creates log folder in current directory
        TextWriter sw = new StreamWriter(logdir + DateTime.Now.ToShortDateString() + " -succesful.txt", true); //creates and starts logging succesful deletions
        foreach (String str in succlog)
        {
            sw.WriteLine(succlog[slogcount]);
            slogcount++;
        }
        sw.Close();
        clearsucclog();
    }

    void faillogger() //logger for failed folders,files deletion
    {
        Directory.CreateDirectory(logdir);//creates log folder in current directory
        TextWriter sw = new StreamWriter(logdir + DateTime.Now.ToShortDateString() + " -failed.txt", true); //creates and starts logging failed deletions
        foreach (String str in faillog)
        {
            sw.WriteLine(faillog[flogcount]);
            flogcount++;
        }
        sw.Close();
        clearfaillog();
    }

    void clearfaillog()//clears failed deletion log array content
    {

            faillog.Clear();
        fitemcount = 1; //resets failed deletion item count 
    }

    void clearsucclog()//clears successful deletion log array content
    {
            succlog.Clear();
        sitemcount = 1; //resets success deletion item count
    }

    void dellogfiles()//deletes log files and disables logging
    {
        //to prevent logging to log this action first disables the logging and after deletion of log file it enables again
        path = logdir;
        disablelogging();
        deleteallfiles();
        enablelogging();
    }

    void disablelogging()
    {
        flogenable = false;
        slogenable = false;

    }
    void enablelogging()
    {
        flogenable = true;
        slogenable = true;
    }



}
}
