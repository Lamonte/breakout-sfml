using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML;
using SFML.Window;
using SFML.Graphics;

namespace SFMLProject
{
	class Program
	{
		static void Main(string[] args)
		{
			Game g = new Game();
			g.Run();
		}
	}
}
