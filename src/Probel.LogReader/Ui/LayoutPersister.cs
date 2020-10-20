using AvalonDock;
using AvalonDock.Layout.Serialization;
using System;
using System.IO;

namespace Probel.LogReader.Ui
{
    public class LayoutPersister
    {
        #region Fields

        private const string _layout = @"<?xml version=""1.0"" encoding=""utf-8""?>
<LayoutRoot>
  <RootPanel Orientation = ""Horizontal"" >
    <LayoutAnchorablePaneGroup Orientation=""Vertical"" DockWidth=""222"">
      <LayoutAnchorablePane Id = ""919ae6d7-f206-4010-a4eb-76c4fb45cd26"" DockWidth=""222"" DockHeight=""1.29429455232858*"" DockMinWidth=""100"">
        <LayoutAnchorable CanHide = ""False"" AutoHideWidth=""200"" AutoHideMinWidth=""100"" AutoHideMinHeight=""100"" Title="" Days "" IsSelected=""True"" ContentId=""_days"" LastActivationTimeStamp=""09/28/2020 12:52:16"" />
      </LayoutAnchorablePane>
      <LayoutAnchorablePaneGroup Orientation = ""Horizontal"" DockHeight=""0.705705447671421*"" FloatingWidth=""232"" FloatingHeight=""947"" FloatingLeft=""109"" FloatingTop=""596"">
        <LayoutAnchorablePane DockMinWidth = ""100"" FloatingWidth=""232"" FloatingHeight=""947"" FloatingLeft=""109"" FloatingTop=""596"">
          <LayoutAnchorable CanHide = ""False"" AutoHideMinWidth=""200"" AutoHideMinHeight=""100"" Title="" Filters "" IsSelected=""True"" ContentId=""_filters"" FloatingLeft=""109"" FloatingTop=""596"" FloatingWidth=""232"" FloatingHeight=""947"" LastActivationTimeStamp=""09/28/2020 12:52:17"" PreviousContainerId=""919ae6d7-f206-4010-a4eb-76c4fb45cd26"" PreviousContainerIndex=""1"" />
        </LayoutAnchorablePane>
      </LayoutAnchorablePaneGroup>
    </LayoutAnchorablePaneGroup>
    <LayoutPanel Orientation = ""Vertical"" DockWidth=""0.93153913492402*"">
      <LayoutDocumentPaneGroup Orientation = ""Horizontal"" >
        <LayoutDocumentPane ShowHeader=""False"">
          <LayoutDocument Title = ""Logs"" IsSelected=""True"" ContentId=""_logs"" CanClose=""False"" />
        </LayoutDocumentPane>
      </LayoutDocumentPaneGroup>
      <LayoutAnchorablePaneGroup Orientation = ""Horizontal"" DockHeight=""400"">
        <LayoutAnchorablePane DockHeight = ""400"" >
          <LayoutAnchorable CanHide=""False"" AutoHideHeight=""400"" AutoHideMinWidth=""100"" AutoHideMinHeight=""100"" CanDockAsTabbedDocument=""False"" Title=""Details"" IsSelected=""True"" ContentId=""_detailPane"" />
          <LayoutAnchorable CanHide = ""False"" AutoHideHeight=""400"" AutoHideMinWidth=""100"" AutoHideMinHeight=""100"" CanDockAsTabbedDocument=""False"" Title=""Message"" ContentId=""_messageDetail"" />
          <LayoutAnchorable CanHide = ""False"" AutoHideHeight=""400"" AutoHideMinWidth=""100"" AutoHideMinHeight=""100"" CanDockAsTabbedDocument=""False"" Title=""Call Stack"" ContentId=""_callstackDetail"" />
        </LayoutAnchorablePane>
      </LayoutAnchorablePaneGroup>
    </LayoutPanel>
  </RootPanel>
  <TopSide />
  <RightSide>
    <LayoutAnchorGroup>
      <LayoutAnchorable CanHide = ""False"" AutoHideMinWidth=""200"" AutoHideMinHeight=""100"" Title="" Repositories "" ContentId=""_repositories"" LastActivationTimeStamp=""09/28/2020 12:52:19"" />
    </LayoutAnchorGroup>
  </RightSide>
  <LeftSide />
  <BottomSide />
  <FloatingWindows />
  <Hidden />
</LayoutRoot>";

        private readonly string _file;

        #endregion Fields

        #region Constructors

        public LayoutPersister(string layoutFile)
        {
            _file = layoutFile;
        }

        #endregion Constructors

        #region Methods

        internal void Delete()
        {
            try
            {
                if (File.Exists(_file))
                {
                    File.Delete(_file);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Impossible to delete the docking layout file of AvalonDock'{_file}'", ex);
            }
        }

        internal void Load(DockingManager dockingManager)
        {
            var serializer = new XmlLayoutSerializer(dockingManager);

            if (File.Exists(_file))
            {
                using (var stream = new StreamReader(_file))
                {
                    serializer.Deserialize(stream);
                }
            }
        }

        internal void ResetLayout(DockingManager dockingManager)
        {
            Delete();
            using (var stream = new StreamWriter(_file))
            {
                stream.Write(_layout);
            }
            Load(dockingManager);
        }

        internal void Save(DockingManager dockingManager)
        {
            var serializer = new XmlLayoutSerializer(dockingManager);

            using (var stream = new StreamWriter(_file))
            {
                serializer.Serialize(stream);
            }
        }

        #endregion Methods
    }
}