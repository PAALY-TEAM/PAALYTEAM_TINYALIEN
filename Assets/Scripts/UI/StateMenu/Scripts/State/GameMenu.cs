namespace UI.StateMenu.Scripts.State
{
    public class GameMenu : _MenuState
    {
        //Specific for this state
        public override void InitState(MenuController menuController)
        {
            base.InitState(menuController);

            state = MenuController.MenuState.Game;
        }
    }
}
