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
	class Brick
	{
		public RectangleShape obj;
		public bool display = true;
		
		public Brick(Color color) {
			obj = new RectangleShape(new Vector2f(150f, 25f));
			obj.FillColor = color;
		}
	}
}
