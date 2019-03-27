using System;

namespace KingOfTheCastle
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (KingOfTheCastle game = new KingOfTheCastle())
            {
                game.Run();
            }
        }
    }
#endif
}

