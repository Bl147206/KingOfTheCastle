using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KingOfTheCastle {
  public abstract class Screen {
    public KingOfTheCastle game;
    public bool isPaused;
    
    public abstract void Update(GameTime gameTime);
    public abstract void Draw(GameTime gameTime);
  }
}
