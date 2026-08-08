using Godot;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

namespace LanMultiplayerMS.Components
{
    internal partial class JoinButton : NJoinFriendRefreshButton
    {
        protected override string[] Hotkeys => [MegaInput.viewMap];

        public static JoinButton Create(NJoinFriendRefreshButton joinFriendRefreshButton)
        {
            var joinButton = new JoinButton
            {
                CustomMinimumSize = new Vector2(150, 50),
                SizeFlagsHorizontal = SizeFlags.ShrinkCenter,
                MouseFilter = MouseFilterEnum.Stop,
                Material = (Material)joinFriendRefreshButton.Material.Duplicate()
            };

            foreach (var child in joinFriendRefreshButton.GetChildren())
                joinButton.AddChildSafely(child.Duplicate());

            var hotkeyIcon = joinButton.FindChild("HotkeyIcon", recursive: true, owned: false) as NHotkeyIcon;
            hotkeyIcon!.Position = new Vector2(hotkeyIcon.Position.X - 12, hotkeyIcon.Position.Y);

            return joinButton;
        }

        public override void _Ready()
        {
            base._Ready();

            var node = GetNode<MegaLabel>("Label");
            node.SetTextAutoSize(new LocString("main_menu_ui", "JOIN.title").GetFormattedText());
        }
    }
}