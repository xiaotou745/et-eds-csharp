namespace TaskPlatform
{
    partial class PlatformForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlatformForm));
            this.tabPageHome = new DevExpress.XtraTab.XtraTabPage();
            this.gpcTotal = new DevExpress.XtraEditors.GroupControl();
            this.lbCompletionThreadsCount = new DevExpress.XtraEditors.LabelControl();
            this.lbWorkThreadsCount = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnCreatePlanV2 = new DevExpress.XtraEditors.SimpleButton();
            this.btnCreateTask = new DevExpress.XtraEditors.SimpleButton();
            this.btnRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.tipController = new DevExpress.Utils.ToolTipController(this.components);
            this.images = new System.Windows.Forms.ImageList(this.components);
            this.lbStartTime = new DevExpress.XtraEditors.LabelControl();
            this.tabContainer = new DevExpress.XtraTab.XtraTabControl();
            this.splitContainer = new DevExpress.XtraEditors.SplitContainerControl();
            this.nbcContainer = new DevExpress.XtraNavBar.NavBarControl();
            this.nbgTasks = new DevExpress.XtraNavBar.NavBarGroup();
            this.nbgPlatformManager = new DevExpress.XtraNavBar.NavBarGroup();
            this.nbiManageTask = new DevExpress.XtraNavBar.NavBarItem();
            this.deadAlarmManager = new DevExpress.XtraNavBar.NavBarItem();
            this.nbiDomainLoaderPerformance = new DevExpress.XtraNavBar.NavBarItem();
            this.nbiDBConfig = new DevExpress.XtraNavBar.NavBarItem();
            this.nbiCacheManager = new DevExpress.XtraNavBar.NavBarItem();
            this.nbiPlatformInfo = new DevExpress.XtraNavBar.NavBarItem();
            this.nbiAbout = new DevExpress.XtraNavBar.NavBarItem();
            this.pgbProcess = new System.Windows.Forms.ProgressBar();
            this.txtInfo = new DevExpress.XtraEditors.TextEdit();
            this.ntfPlatformNotify = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiShow = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHide = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.bsubMoveToGroup = new DevExpress.XtraBars.BarSubItem();
            this.btnUnloadTask = new DevExpress.XtraBars.BarButtonItem();
            this.btnOpenTaskStage = new DevExpress.XtraBars.BarButtonItem();
            this.btnAddGroup = new DevExpress.XtraBars.BarButtonItem();
            this.bsubDeleteGroup = new DevExpress.XtraBars.BarSubItem();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem3 = new DevExpress.XtraBars.BarStaticItem();
            this.bsubRenameGroup = new DevExpress.XtraBars.BarSubItem();
            this.barbtnOpenDirectory = new DevExpress.XtraBars.BarButtonItem();
            this.barbtnReload = new DevExpress.XtraBars.BarButtonItem();
            this.popupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            this.tabPageHome.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gpcTotal)).BeginInit();
            this.gpcTotal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabContainer)).BeginInit();
            this.tabContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbcContainer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInfo.Properties)).BeginInit();
            this.cmsMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // tabPageHome
            // 
            this.tabPageHome.Controls.Add(this.gpcTotal);
            this.tabPageHome.ImageIndex = 0;
            this.tabPageHome.Name = "tabPageHome";
            this.tabPageHome.ShowCloseButton = DevExpress.Utils.DefaultBoolean.False;
            this.tabPageHome.Size = new System.Drawing.Size(653, 470);
            this.tabPageHome.Text = "平台信息";
            // 
            // gpcTotal
            // 
            this.gpcTotal.Controls.Add(this.lbCompletionThreadsCount);
            this.gpcTotal.Controls.Add(this.lbWorkThreadsCount);
            this.gpcTotal.Controls.Add(this.labelControl2);
            this.gpcTotal.Controls.Add(this.labelControl1);
            this.gpcTotal.Controls.Add(this.btnCreatePlanV2);
            this.gpcTotal.Controls.Add(this.btnCreateTask);
            this.gpcTotal.Controls.Add(this.btnRefresh);
            this.gpcTotal.Controls.Add(this.lbStartTime);
            this.gpcTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpcTotal.Location = new System.Drawing.Point(0, 0);
            this.gpcTotal.Name = "gpcTotal";
            this.gpcTotal.Size = new System.Drawing.Size(653, 470);
            this.gpcTotal.TabIndex = 0;
            this.gpcTotal.Text = "总体信息";
            // 
            // lbCompletionThreadsCount
            // 
            this.lbCompletionThreadsCount.Location = new System.Drawing.Point(205, 83);
            this.lbCompletionThreadsCount.Name = "lbCompletionThreadsCount";
            this.lbCompletionThreadsCount.Size = new System.Drawing.Size(7, 14);
            this.lbCompletionThreadsCount.TabIndex = 5;
            this.lbCompletionThreadsCount.Text = "0";
            // 
            // lbWorkThreadsCount
            // 
            this.lbWorkThreadsCount.Location = new System.Drawing.Point(179, 63);
            this.lbWorkThreadsCount.Name = "lbWorkThreadsCount";
            this.lbWorkThreadsCount.Size = new System.Drawing.Size(7, 14);
            this.lbWorkThreadsCount.TabIndex = 5;
            this.lbWorkThreadsCount.Text = "0";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(29, 83);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(170, 14);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "池中可用异步 I/O 线程的数目：";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(29, 63);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(144, 14);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "池中可用辅助线程的数目：";
            // 
            // btnCreatePlanV2
            // 
            this.btnCreatePlanV2.Location = new System.Drawing.Point(284, 26);
            this.btnCreatePlanV2.Name = "btnCreatePlanV2";
            this.btnCreatePlanV2.Size = new System.Drawing.Size(65, 20);
            this.btnCreatePlanV2.TabIndex = 3;
            this.btnCreatePlanV2.Text = "创建计划";
            this.btnCreatePlanV2.Click += new System.EventHandler(this.btnCreatePlanV2_Click);
            // 
            // btnCreateTask
            // 
            this.btnCreateTask.Location = new System.Drawing.Point(197, 26);
            this.btnCreateTask.Name = "btnCreateTask";
            this.btnCreateTask.Size = new System.Drawing.Size(65, 20);
            this.btnCreateTask.TabIndex = 2;
            this.btnCreateTask.Text = "创建任务";
            this.btnCreateTask.Click += new System.EventHandler(this.btnCreateTask_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(88, 26);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(89, 20);
            toolTipTitleItem1.Appearance.Image = global::TaskPlatform.Properties.Resources.Messages;
            toolTipTitleItem1.Appearance.Options.UseImage = true;
            toolTipTitleItem1.Image = global::TaskPlatform.Properties.Resources.Messages;
            toolTipTitleItem1.Text = "刷新计划任务";
            toolTipItem1.LeftIndent = 6;
            toolTipItem1.Text = "      立刻刷新计划任务。";
            superToolTip1.Items.Add(toolTipTitleItem1);
            superToolTip1.Items.Add(toolTipItem1);
            this.btnRefresh.SuperTip = superToolTip1;
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "刷新计划任务";
            this.btnRefresh.ToolTip = "立刻刷新计划任务。";
            this.btnRefresh.ToolTipController = this.tipController;
            this.btnRefresh.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnRefresh.ToolTipTitle = "刷新计划任务";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // tipController
            // 
            this.tipController.AllowHtmlText = true;
            this.tipController.AutoPopDelay = 10000;
            this.tipController.CloseOnClick = DevExpress.Utils.DefaultBoolean.True;
            this.tipController.ImageIndex = 3;
            this.tipController.ImageList = this.images;
            this.tipController.InitialDelay = 100;
            this.tipController.ToolTipType = DevExpress.Utils.ToolTipType.SuperTip;
            // 
            // images
            // 
            this.images.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("images.ImageStream")));
            this.images.TransparentColor = System.Drawing.Color.Transparent;
            this.images.Images.SetKeyName(0, "Browser");
            this.images.Images.SetKeyName(1, "Calender");
            this.images.Images.SetKeyName(2, "Clock");
            this.images.Images.SetKeyName(3, "Messages");
            this.images.Images.SetKeyName(4, "Camera");
            this.images.Images.SetKeyName(5, "Gmail");
            this.images.Images.SetKeyName(6, "Music");
            this.images.Images.SetKeyName(7, "Error");
            this.images.Images.SetKeyName(8, "Error2");
            this.images.Images.SetKeyName(9, "Right");
            this.images.Images.SetKeyName(10, "Wait");
            this.images.Images.SetKeyName(11, "Annotate_Complete.ico");
            this.images.Images.SetKeyName(12, "Annotate_Default.ico");
            this.images.Images.SetKeyName(13, "Annotate_Error.ico");
            this.images.Images.SetKeyName(14, "Annotate_Warning.ico");
            this.images.Images.SetKeyName(15, "042b_AddCategory.ico");
            this.images.Images.SetKeyName(16, "075b_UpFolder.ico");
            this.images.Images.SetKeyName(17, "125_FullView.ico");
            this.images.Images.SetKeyName(18, "berror.ico");
            this.images.Images.SetKeyName(19, "Close.ico");
            this.images.Images.SetKeyName(20, "delete.ico");
            this.images.Images.SetKeyName(21, "move.ico");
            this.images.Images.SetKeyName(22, "Rename.ico");
            this.images.Images.SetKeyName(23, "Annotate_info.ico");
            // 
            // lbStartTime
            // 
            this.lbStartTime.Location = new System.Drawing.Point(5, 26);
            this.lbStartTime.Name = "lbStartTime";
            this.lbStartTime.Size = new System.Drawing.Size(48, 14);
            this.lbStartTime.TabIndex = 0;
            this.lbStartTime.Text = "信息暂缺";
            // 
            // tabContainer
            // 
            this.tabContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabContainer.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InActiveTabPageAndTabControlHeader;
            this.tabContainer.HeaderButtons = ((DevExpress.XtraTab.TabButtons)((DevExpress.XtraTab.TabButtons.Close | DevExpress.XtraTab.TabButtons.Default)));
            this.tabContainer.Images = this.images;
            this.tabContainer.Location = new System.Drawing.Point(0, 0);
            this.tabContainer.Name = "tabContainer";
            this.tabContainer.SelectedTabPage = this.tabPageHome;
            this.tabContainer.Size = new System.Drawing.Size(659, 499);
            this.tabContainer.TabIndex = 0;
            this.tabContainer.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabPageHome});
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Panel1.Controls.Add(this.nbcContainer);
            this.splitContainer.Panel1.Text = "Panel1";
            this.splitContainer.Panel2.Controls.Add(this.pgbProcess);
            this.splitContainer.Panel2.Controls.Add(this.txtInfo);
            this.splitContainer.Panel2.Controls.Add(this.tabContainer);
            this.splitContainer.Panel2.Text = "Panel2";
            this.splitContainer.Size = new System.Drawing.Size(818, 515);
            this.splitContainer.SplitterPosition = 154;
            this.splitContainer.TabIndex = 1;
            this.splitContainer.Text = "splitContainerControl1";
            // 
            // nbcContainer
            // 
            this.nbcContainer.ActiveGroup = this.nbgTasks;
            this.nbcContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nbcContainer.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.nbgTasks,
            this.nbgPlatformManager});
            this.nbcContainer.HideGroupCaptions = true;
            this.nbcContainer.Items.AddRange(new DevExpress.XtraNavBar.NavBarItem[] {
            this.nbiPlatformInfo,
            this.nbiAbout,
            this.nbiManageTask,
            this.nbiDBConfig,
            this.nbiCacheManager,
            this.nbiDomainLoaderPerformance,
            this.deadAlarmManager});
            this.nbcContainer.Location = new System.Drawing.Point(0, 0);
            this.nbcContainer.Name = "nbcContainer";
            this.nbcContainer.NavigationPaneMaxVisibleGroups = 4;
            this.nbcContainer.OptionsNavPane.ExpandedWidth = 154;
            this.nbcContainer.OptionsNavPane.ShowExpandButton = false;
            this.nbcContainer.ShowHintInterval = 200;
            this.nbcContainer.Size = new System.Drawing.Size(154, 515);
            this.nbcContainer.SmallImages = this.images;
            this.nbcContainer.TabIndex = 0;
            this.nbcContainer.Text = "平台向导";
            this.nbcContainer.ToolTipController = this.tipController;
            this.nbcContainer.View = new DevExpress.XtraNavBar.ViewInfo.StandardSkinNavigationPaneViewInfoRegistrator("Black");
            // 
            // nbgTasks
            // 
            this.nbgTasks.Caption = "计划任务列表";
            this.nbgTasks.Expanded = true;
            this.nbgTasks.Hint = "列出默认分组的计划任务信息";
            this.nbgTasks.Name = "nbgTasks";
            this.nbgTasks.SmallImageIndex = 1;
            // 
            // nbgPlatformManager
            // 
            this.nbgPlatformManager.Caption = "平台管理";
            this.nbgPlatformManager.DragDropFlags = ((DevExpress.XtraNavBar.NavBarDragDrop)((DevExpress.XtraNavBar.NavBarDragDrop.Default | DevExpress.XtraNavBar.NavBarDragDrop.AllowDrag)));
            this.nbgPlatformManager.Hint = "管理平台中的计划任务，查看平台信息";
            this.nbgPlatformManager.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbiManageTask),
            new DevExpress.XtraNavBar.NavBarItemLink(this.deadAlarmManager),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbiDomainLoaderPerformance),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbiDBConfig),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbiCacheManager),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbiPlatformInfo),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbiAbout)});
            this.nbgPlatformManager.Name = "nbgPlatformManager";
            this.nbgPlatformManager.SmallImageIndex = 0;
            this.nbgPlatformManager.Tag = "PaltformManagementGroup";
            // 
            // nbiManageTask
            // 
            this.nbiManageTask.Caption = "管理计划任务";
            this.nbiManageTask.Hint = "查看所有计划的文件信息并对所有计划任务进行管理";
            this.nbiManageTask.Name = "nbiManageTask";
            this.nbiManageTask.SmallImageIndex = 2;
            this.nbiManageTask.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.nbiManageTask_LinkClicked);
            // 
            // deadAlarmManager
            // 
            this.deadAlarmManager.Caption = "死信警报管理";
            this.deadAlarmManager.Hint = "管理有警报但没有相对应的任务的警报信息";
            this.deadAlarmManager.Name = "deadAlarmManager";
            this.deadAlarmManager.SmallImageIndex = 4;
            this.deadAlarmManager.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.deadAlarmManager_LinkClicked);
            // 
            // nbiDomainLoaderPerformance
            // 
            this.nbiDomainLoaderPerformance.Caption = "性能监测";
            this.nbiDomainLoaderPerformance.Hint = "对计划任务平台及包含的所有任务执行性能检测";
            this.nbiDomainLoaderPerformance.Name = "nbiDomainLoaderPerformance";
            this.nbiDomainLoaderPerformance.SmallImageIndex = 4;
            this.nbiDomainLoaderPerformance.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.nbiDomainLoaderPerformance_LinkClicked);
            // 
            // nbiDBConfig
            // 
            this.nbiDBConfig.Caption = "数据库设置";
            this.nbiDBConfig.Hint = "管理平台使用的所有数据库连接信息";
            this.nbiDBConfig.Name = "nbiDBConfig";
            this.nbiDBConfig.SmallImageIndex = 4;
            this.nbiDBConfig.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.nbiDBConfig_LinkClicked);
            // 
            // nbiCacheManager
            // 
            this.nbiCacheManager.Caption = "缓存管理器";
            this.nbiCacheManager.Hint = "管理平台的所有缓存";
            this.nbiCacheManager.Name = "nbiCacheManager";
            this.nbiCacheManager.SmallImageIndex = 1;
            this.nbiCacheManager.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.nbiCacheManager_LinkClicked);
            // 
            // nbiPlatformInfo
            // 
            this.nbiPlatformInfo.CanDrag = false;
            this.nbiPlatformInfo.Caption = "平台信息";
            this.nbiPlatformInfo.Hint = "查看平台运行信息";
            this.nbiPlatformInfo.Name = "nbiPlatformInfo";
            this.nbiPlatformInfo.SmallImageIndex = 3;
            this.nbiPlatformInfo.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.nbiPlatformInfo_LinkClicked);
            // 
            // nbiAbout
            // 
            this.nbiAbout.CanDrag = false;
            this.nbiAbout.Caption = "   关于   ";
            this.nbiAbout.Hint = "查看平台的版本相关信息";
            this.nbiAbout.Name = "nbiAbout";
            this.nbiAbout.SmallImageIndex = 0;
            this.nbiAbout.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.nbiAbout_LinkClicked);
            // 
            // pgbProcess
            // 
            this.pgbProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pgbProcess.Location = new System.Drawing.Point(380, 499);
            this.pgbProcess.Name = "pgbProcess";
            this.pgbProcess.Size = new System.Drawing.Size(279, 14);
            this.pgbProcess.TabIndex = 2;
            this.pgbProcess.Visible = false;
            // 
            // txtInfo
            // 
            this.txtInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtInfo.EditValue = "主框架已启动，等待加载计划任务……";
            this.txtInfo.Location = new System.Drawing.Point(0, 494);
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.Properties.ReadOnly = true;
            this.txtInfo.Properties.UseParentBackground = true;
            this.txtInfo.Size = new System.Drawing.Size(659, 21);
            this.txtInfo.TabIndex = 1;
            this.txtInfo.ToolTip = "显示常规信息";
            this.txtInfo.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.txtInfo.ToolTipTitle = "提示";
            // 
            // ntfPlatformNotify
            // 
            this.ntfPlatformNotify.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ntfPlatformNotify.BalloonTipText = "计划任务平台正在运行";
            this.ntfPlatformNotify.BalloonTipTitle = "提示";
            this.ntfPlatformNotify.ContextMenuStrip = this.cmsMenu;
            this.ntfPlatformNotify.Icon = ((System.Drawing.Icon)(resources.GetObject("ntfPlatformNotify.Icon")));
            this.ntfPlatformNotify.Text = "计划任务平台通知";
            this.ntfPlatformNotify.Visible = true;
            this.ntfPlatformNotify.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ntfPlatformNotify_MouseClick);
            // 
            // cmsMenu
            // 
            this.cmsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiShow,
            this.tsmiHide,
            this.toolStripMenuItem1,
            this.tsmiExit});
            this.cmsMenu.Name = "cmsMenu";
            this.cmsMenu.Size = new System.Drawing.Size(101, 76);
            // 
            // tsmiShow
            // 
            this.tsmiShow.Name = "tsmiShow";
            this.tsmiShow.Size = new System.Drawing.Size(100, 22);
            this.tsmiShow.Text = "显示";
            this.tsmiShow.Click += new System.EventHandler(this.tsmiShow_Click);
            // 
            // tsmiHide
            // 
            this.tsmiHide.Name = "tsmiHide";
            this.tsmiHide.Size = new System.Drawing.Size(100, 22);
            this.tsmiHide.Text = "隐藏";
            this.tsmiHide.Click += new System.EventHandler(this.tsmiHide_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(97, 6);
            // 
            // tsmiExit
            // 
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new System.Drawing.Size(100, 22);
            this.tsmiExit.Text = "退出";
            this.tsmiExit.Click += new System.EventHandler(this.tsmiExit_Click);
            // 
            // barManager
            // 
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Images = this.images;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.bsubMoveToGroup,
            this.btnUnloadTask,
            this.btnOpenTaskStage,
            this.btnAddGroup,
            this.bsubDeleteGroup,
            this.barStaticItem1,
            this.barStaticItem2,
            this.barStaticItem3,
            this.bsubRenameGroup,
            this.barbtnOpenDirectory,
            this.barbtnReload});
            this.barManager.LargeImages = this.images;
            this.barManager.MaxItemId = 14;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(818, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 515);
            this.barDockControlBottom.Size = new System.Drawing.Size(818, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 515);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(818, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 515);
            // 
            // bsubMoveToGroup
            // 
            this.bsubMoveToGroup.Caption = "移动到分组";
            this.bsubMoveToGroup.Id = 0;
            this.bsubMoveToGroup.ImageIndex = 16;
            this.bsubMoveToGroup.Name = "bsubMoveToGroup";
            // 
            // btnUnloadTask
            // 
            this.btnUnloadTask.Caption = "卸载计划任务";
            this.btnUnloadTask.Id = 1;
            this.btnUnloadTask.ImageIndex = 19;
            this.btnUnloadTask.Name = "btnUnloadTask";
            this.btnUnloadTask.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUnloadTask_ItemClick);
            // 
            // btnOpenTaskStage
            // 
            this.btnOpenTaskStage.Caption = "查看计划任务";
            this.btnOpenTaskStage.Id = 2;
            this.btnOpenTaskStage.ImageIndex = 17;
            this.btnOpenTaskStage.Name = "btnOpenTaskStage";
            this.btnOpenTaskStage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOpenTaskStage_ItemClick);
            // 
            // btnAddGroup
            // 
            this.btnAddGroup.Caption = "新增分组";
            this.btnAddGroup.Id = 4;
            this.btnAddGroup.ImageIndex = 15;
            this.btnAddGroup.Name = "btnAddGroup";
            this.btnAddGroup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAddGroup_ItemClick);
            // 
            // bsubDeleteGroup
            // 
            this.bsubDeleteGroup.Caption = "删除分组";
            this.bsubDeleteGroup.Id = 5;
            this.bsubDeleteGroup.ImageIndex = 20;
            this.bsubDeleteGroup.Name = "bsubDeleteGroup";
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "barStaticItem1";
            this.barStaticItem1.Id = 7;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticItem2
            // 
            this.barStaticItem2.Caption = "-------------";
            this.barStaticItem2.Id = 8;
            this.barStaticItem2.Name = "barStaticItem2";
            this.barStaticItem2.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticItem3
            // 
            this.barStaticItem3.Caption = "-------------";
            this.barStaticItem3.Id = 9;
            this.barStaticItem3.Name = "barStaticItem3";
            this.barStaticItem3.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // bsubRenameGroup
            // 
            this.bsubRenameGroup.Caption = "重命名分组";
            this.bsubRenameGroup.Id = 11;
            this.bsubRenameGroup.ImageIndex = 22;
            this.bsubRenameGroup.Name = "bsubRenameGroup";
            // 
            // barbtnOpenDirectory
            // 
            this.barbtnOpenDirectory.Caption = "打开目录";
            this.barbtnOpenDirectory.Id = 12;
            this.barbtnOpenDirectory.ImageIndex = 16;
            this.barbtnOpenDirectory.Name = "barbtnOpenDirectory";
            this.barbtnOpenDirectory.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnOpenDirectory_ItemClick);
            // 
            // barbtnReload
            // 
            this.barbtnReload.Caption = "重新加载";
            this.barbtnReload.Id = 13;
            this.barbtnReload.ImageIndex = 18;
            this.barbtnReload.Name = "barbtnReload";
            this.barbtnReload.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnReload_ItemClick);
            // 
            // popupMenu
            // 
            this.popupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnOpenTaskStage, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.barbtnOpenDirectory, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barbtnReload, true),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnUnloadTask, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnAddGroup, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.bsubMoveToGroup, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.bsubDeleteGroup, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.bsubRenameGroup, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.popupMenu.Manager = this.barManager;
            this.popupMenu.Name = "popupMenu";
            this.popupMenu.CloseUp += new System.EventHandler(this.popupMenu_CloseUp);
            // 
            // PlatformForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 515);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PlatformForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "计划任务平台";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PlatformForm_FormClosing);
            this.Load += new System.EventHandler(this.PlatformForm_Load);
            this.SizeChanged += new System.EventHandler(this.PlatformForm_SizeChanged);
            this.tabPageHome.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gpcTotal)).EndInit();
            this.gpcTotal.ResumeLayout(false);
            this.gpcTotal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabContainer)).EndInit();
            this.tabContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nbcContainer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInfo.Properties)).EndInit();
            this.cmsMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabPage tabPageHome;
        private DevExpress.XtraTab.XtraTabControl tabContainer;
        private DevExpress.XtraEditors.SplitContainerControl splitContainer;
        private DevExpress.XtraNavBar.NavBarControl nbcContainer;
        private DevExpress.XtraNavBar.NavBarGroup nbgPlatformManager;
        private DevExpress.XtraNavBar.NavBarGroup nbgTasks;
        private DevExpress.XtraEditors.TextEdit txtInfo;
        private System.Windows.Forms.ProgressBar pgbProcess;
        private DevExpress.XtraNavBar.NavBarItem nbiPlatformInfo;
        private DevExpress.XtraNavBar.NavBarItem nbiAbout;
        private System.Windows.Forms.ImageList images;
        private DevExpress.Utils.ToolTipController tipController;
        private DevExpress.XtraEditors.GroupControl gpcTotal;
        private DevExpress.XtraEditors.LabelControl lbStartTime;
        private DevExpress.XtraNavBar.NavBarItem nbiManageTask;
        private DevExpress.XtraNavBar.NavBarItem nbiDBConfig;
        private DevExpress.XtraNavBar.NavBarItem nbiCacheManager;
        private DevExpress.XtraEditors.SimpleButton btnRefresh;
        private DevExpress.XtraEditors.SimpleButton btnCreateTask;
        private DevExpress.XtraNavBar.NavBarItem nbiDomainLoaderPerformance;
        private System.Windows.Forms.NotifyIcon ntfPlatformNotify;
        private System.Windows.Forms.ContextMenuStrip cmsMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiShow;
        private System.Windows.Forms.ToolStripMenuItem tsmiHide;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsmiExit;
        private DevExpress.XtraNavBar.NavBarItem deadAlarmManager;
        private DevExpress.XtraEditors.SimpleButton btnCreatePlanV2;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl lbWorkThreadsCount;
        private DevExpress.XtraEditors.LabelControl lbCompletionThreadsCount;
        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarSubItem bsubMoveToGroup;
        private DevExpress.XtraBars.BarButtonItem btnUnloadTask;
        private DevExpress.XtraBars.PopupMenu popupMenu;
        private DevExpress.XtraBars.BarButtonItem btnOpenTaskStage;
        private DevExpress.XtraBars.BarButtonItem btnAddGroup;
        private DevExpress.XtraBars.BarSubItem bsubDeleteGroup;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem2;
        private DevExpress.XtraBars.BarStaticItem barStaticItem3;
        private DevExpress.XtraBars.BarSubItem bsubRenameGroup;
        private DevExpress.XtraBars.BarButtonItem barbtnOpenDirectory;
        private DevExpress.XtraBars.BarButtonItem barbtnReload;

    }
}