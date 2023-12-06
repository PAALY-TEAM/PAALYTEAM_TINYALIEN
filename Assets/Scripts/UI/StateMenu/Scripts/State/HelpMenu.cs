namespace UI.StateMenu.Scripts.State
{
    public class HelpMenu : _MenuState
    {
        //Specific for this state
        public override void InitState(MenuController menuController)
        {
            base.InitState(menuController);

            state = MenuController.MenuState.Help;
        }
    }
}
