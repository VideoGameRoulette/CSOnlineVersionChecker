using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CSOnlineVersionChecker
{
    public class OnlineVersion
    {
        public enum UpdateType
        {
            None,
            MinorBugFix,
            MajorBugFix,
            MinorUpdate,
            MajorUpdate
        }

        public static string PageAddress = "https://videogameroulette.github.io/CSOnlineVersionChecker/";

        public static void CheckUpdate()
        {
            try
            {
                string Response = GetResponse(PageAddress);
                if (string.IsNullOrWhiteSpace(Response)) { } //Do Nothing
                const string pattern = @"Current Version: (?<version>\d.\d.\d.\d)";
                Match TempMatch = Regex.Match(Response, pattern);
                if (TempMatch.Success)
                {
                    var CurrentVersion = TempMatch.Groups["version"].Value;
                    string[] CurrentVersionTemp = CurrentVersion.Split('.');
                    List<int> CurrentVersionList = new List<int>();
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                    var LocalVersion = fvi.FileVersion;
                    string[] LocalVersionTemp = LocalVersion.Split('.');
                    List<int> LocalVersionList = new List<int>();
                    for (int s = 0; s <= 3; s++) { CurrentVersionList.Add(Convert.ToInt32(CurrentVersionTemp[s])); LocalVersionList.Add(Convert.ToInt32(LocalVersionTemp[s])); }
                    int[] CurrentVersionInt = { CurrentVersionList[0], CurrentVersionList[1], CurrentVersionList[2], CurrentVersionList[3] };
                    int[] LocalVersionInt = { LocalVersionList[0], LocalVersionList[1], LocalVersionList[2], LocalVersionList[3] };
                    UpdateType ChangeType;
                    ChangeType = GetUpdateType(CurrentVersionInt, LocalVersionInt);
                    if (ChangeType != UpdateType.None)
                    {
                        var result = MessageBox.Show($"You have version {LocalVersion}. Version {GetUpdateString(ChangeType)} version {CurrentVersion} {Environment.NewLine} Would you like to update?", "Version Update", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes) { Help.ShowHelp(null, PageAddress); }
                        else { } //Do nothing.
                    }
                    else { MessageBox.Show("Version up to date."); } //Do nothing.
                }
            }
            catch (NullReferenceException ex) { MessageBox.Show(ex.ToString()); }
        }

        public static string GetResponse(string Address)
        {
            if (!Address.Contains("https://videogameroulette.github.io/CSOnlineVersionChecker/")) { return null; }
            WebBrowser TempBrowser = new WebBrowser() { ScrollBarsEnabled = false, ScriptErrorsSuppressed = true };
            TempBrowser.Navigate(Address);
            while ((TempBrowser.ReadyState == WebBrowserReadyState.Complete) == false) { Application.DoEvents(); }
            return TempBrowser.Document.Body.InnerHtml;
        }

        public static UpdateType GetUpdateType(int[] CheckedVersion, int[] LocalVersion)
        {
            if (CheckedVersion[0] > LocalVersion[0]) { return UpdateType.MajorUpdate; }
            else if (CheckedVersion[0] >= LocalVersion[0] && CheckedVersion[1] > LocalVersion[1]) { return UpdateType.MinorUpdate; }
            else if (CheckedVersion[0] >= LocalVersion[0] && CheckedVersion[1] >= LocalVersion[1] && CheckedVersion[2] > LocalVersion[2]) { return UpdateType.MajorBugFix; }
            else if (CheckedVersion[0] >= LocalVersion[0] && CheckedVersion[1] >= LocalVersion[1] && CheckedVersion[2] >= LocalVersion[2] && CheckedVersion[3] > LocalVersion[3]) { return UpdateType.MinorBugFix; }
            else { return UpdateType.None; }
        }

        public static object GetUpdateString(UpdateType ChangeType)
        {
            if (ChangeType == UpdateType.MajorUpdate) { return "Major Update"; }
            else if (ChangeType == UpdateType.MinorUpdate) { return "Minor Update"; }
            else if (ChangeType == UpdateType.MajorBugFix) { return "Major Bug Fix"; }
            else if (ChangeType == UpdateType.MinorBugFix) { return "Minor Bug Fix"; }
            else { return null; }
        }

        //For Debugging Purposes
        public static string GetLocalVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            var LocalVersion = fvi.FileVersion;
            return LocalVersion;
        }

        public static string GetOnlineVersion()
        {
            try
            {
                string Response = GetResponse(PageAddress);
                if (string.IsNullOrWhiteSpace(Response)) { } //Do Nothing
                const string pattern = @"Current Version: (?<version>\d.\d.\d.\d)";
                Match TempMatch = Regex.Match(Response, pattern);
                if (TempMatch.Success)
                {
                    var CurrentVersion = TempMatch.Groups["version"].Value;
                    return CurrentVersion;
                }
                return null;
            }
            catch (NullReferenceException ex) { MessageBox.Show(ex.ToString()); return null; }
        }

        public static string GetUpdateTypeString()
        {
            try
            {
                string Response = GetResponse(PageAddress);
                if (string.IsNullOrWhiteSpace(Response)) { } //Do Nothing
                const string pattern = @"Current Version: (?<version>\d.\d.\d.\d)";
                Match TempMatch = Regex.Match(Response, pattern);
                if (TempMatch.Success)
                {
                    var CurrentVersion = TempMatch.Groups["version"].Value;
                    string[] CurrentVersionTemp = CurrentVersion.Split('.');
                    List<int> CurrentVersionList = new List<int>();
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                    var LocalVersion = fvi.FileVersion;
                    string[] LocalVersionTemp = LocalVersion.Split('.');
                    List<int> LocalVersionList = new List<int>();
                    for (int s = 0; s <= 3; s++) { CurrentVersionList.Add(Convert.ToInt32(CurrentVersionTemp[s])); LocalVersionList.Add(Convert.ToInt32(LocalVersionTemp[s])); }
                    int[] CurrentVersionInt = { CurrentVersionList[0], CurrentVersionList[1], CurrentVersionList[2], CurrentVersionList[3] };
                    int[] LocalVersionInt = { LocalVersionList[0], LocalVersionList[1], LocalVersionList[2], LocalVersionList[3] };
                    UpdateType ChangeType;
                    ChangeType = GetUpdateType(CurrentVersionInt, LocalVersionInt);
                    return ChangeType.ToString();
                }
                return null;
            }
            catch (NullReferenceException ex) { MessageBox.Show(ex.ToString()); return null; }
        }
    }
}