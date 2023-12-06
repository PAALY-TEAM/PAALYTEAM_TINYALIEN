namespace UI.StateMenu.Scripts.State
{
    public class MainMenu : _MenuState
    {
        //Specific for this state
        public override void InitState(MenuController menuController)
        {
            base.InitState(menuController);

            state = MenuController.MenuState.Main;
        }

        public void JumpToSettings()
        {
            menuController.SetActiveState(MenuController.MenuState.Settings);
        }

        public void JumpToHelp()
        {
            menuController.SetActiveState(MenuController.MenuState.Help);
        }

        public void QuitGame()
        {
            menuController.QuitGame();
        }
    }
}
