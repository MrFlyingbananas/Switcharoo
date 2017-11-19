using Microsoft.Xna.Framework.Input;

namespace Switcharoo
{
    public class Controls
    {
        public Keys UpKey { get; private set; }
        public Keys DownKey { get; private set; }
        public Keys LeftKey { get; private set; }
        public Keys RightKey { get; private set; }
        public Keys FireKey { get; private set; }
         
        public Controls(Keys left, Keys right, Keys up, Keys down, Keys Fire)
        {
            UpKey = up;
            RightKey = right;
            LeftKey = left;
            DownKey = down;
            FireKey = Fire;
        }

    }


}