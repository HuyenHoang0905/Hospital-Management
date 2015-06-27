using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for HTMLHelp.
	/// </summary>
	internal class HTMLHelp
	{
		#region API_Declaration
		// HH_DISPLAY_SEARCH Command Related Structure
		[StructLayout(LayoutKind.Sequential)]
			public struct tagHH_FTS_QUERY
		{
			long cbStruct;
			long fUniCodeStrings;
			string pszSearchQuery;
			long iProximity;
			long fStemmedSearch;
			long fTitleOnly;
			long fExecute;
			string pszWindow;
		}
		[DllImport("hhctrl.ocx",SetLastError=true, CharSet=CharSet.Ansi, EntryPoint="HtmlHelpA")]
		private static extern IntPtr HtmlHelp(IntPtr hwnd, string lpHelpFile, int wCommand, int dwData);
		[DllImport("hhctrl.ocx",SetLastError=true, CharSet=CharSet.Auto, EntryPoint="HtmlHelp")]
        private static extern IntPtr HTMLHelpFtsQuery (IntPtr hwnd, string lpHelpFile, int wCommand, ref tagHH_FTS_QUERY dwData );
        private const int HH_DISPLAY_TOPIC = 0x0;
        private const int HH_DISPLAY_TOC = 0x1;
        private const int HH_DISPLAY_INDEX = 0x2;
        private const int HH_DISPLAY_SEARCH = 0x3;
        private const int HH_HELP_CONTEXT = 0xF;   // display mapped numeric value in dwData

		[DllImport("user32")]
		private static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[DllImport("user32")]
		private static extern bool IsWindow(IntPtr hWnd);

		public const int WM_CLOSE=0x10;

        #endregion
		
		private Control m_ParentWindow=null;
		private string m_HelpFile="";
		private IntPtr m_HtmlHelpWindowHandle=IntPtr.Zero;

		public HTMLHelp()
		{}
		public HTMLHelp(Control parent, string helpfile)
		{
			m_ParentWindow=parent;
			m_HelpFile=helpfile;
		}
		public Control Parent
		{
			get {return m_ParentWindow;}
			set {m_ParentWindow=value;}
		}
		public string HelpFile
		{
			get {return m_HelpFile;}
			set {m_HelpFile=value;}
		}

		public void ShowContents()
		{
			// Display Table Of Contents tab in HTML Help
			if(File.Exists(m_HelpFile))
				m_HtmlHelpWindowHandle=HtmlHelp(m_ParentWindow.Handle,m_HelpFile,HH_DISPLAY_TOC,0);
			else
				throw new InvalidOperationException("Help file could not be found.");
		}

		public void ShowIndex()
		{
			// Display Table Of Contents tab in HTML Help
			if(File.Exists(m_HelpFile))
				m_HtmlHelpWindowHandle=HtmlHelp(m_ParentWindow.Handle,m_HelpFile,HH_DISPLAY_INDEX,0);
			else
				throw new InvalidOperationException("Help file could not be found.");
		}

		public void ShowSearch()
		{
			// Display Table Of Contents tab in HTML Help
			if(File.Exists(m_HelpFile))
				m_HtmlHelpWindowHandle=HtmlHelp(m_ParentWindow.Handle,m_HelpFile,HH_DISPLAY_SEARCH,0);
			else
				throw new InvalidOperationException("Help file could not be found.");
		}

		public void CloseHelpWindow()
		{
			if(m_HtmlHelpWindowHandle!=IntPtr.Zero && IsWindow(m_HtmlHelpWindowHandle))
				PostMessage(m_HtmlHelpWindowHandle,WM_CLOSE,0,0);
			m_HtmlHelpWindowHandle=IntPtr.Zero;
		}
	}
}
