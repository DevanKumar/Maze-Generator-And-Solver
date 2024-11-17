using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DiscreteStructuresAE2
{
    // InputManager is a static class that holds information or helper
    // functions that have global importance/use
    internal static class InputManager
    {
        public struct PointNode
        {
            public bool Created { get; set; }
            public Vector2 Position { get; set; }
        }
        public static MouseState CurrentMouseState { get; set; }
        public static MouseState PreviousMouseState { get; set; }
        public static Color NewColor { get; set; }
        public static PointNode StartNode = new PointNode();
        public static PointNode EndNode = new PointNode();
        public static bool Generated { get; set; }
        public static Vector2 MousePosition()
        {
            return new Vector2(CurrentMouseState.X, CurrentMouseState.Y);
        }
        
        public static bool LeftClicked()
        {
            if(PreviousMouseState.LeftButton == ButtonState.Pressed && CurrentMouseState.LeftButton == ButtonState.Released)
            {
                return true;
            }
            return false;
        }
    }
}
