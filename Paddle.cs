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
	class Paddle
	{
		public Color color;
		public RectangleShape obj;
		public bool display = true;
		public float x_position = 0f;
		public float y_position = 0f;

		//get the x and y position
		public float x {
			get { return obj.Position.X; }
			set { set_position(value, y); }
		}

		public float y {
			get { return obj.Position.Y; }
			set { set_position(x, value); }
		}

		//new paddle instance; w = width, h = height
		public Paddle(float w, float h) {
			obj = new RectangleShape(new Vector2f(w, h));
		}

		//with color parameter
		public Paddle(float w, float h, Color color) {
			obj = new RectangleShape(new Vector2f(w, h));
			set_color(color);
		}

		//set the color of the paddle
		public void set_color(Color color) {
			obj.FillColor = color;
		}

		//set paddle position
		public void set_position(float x, float y) {
			obj.Position = new Vector2f(x, y);
		}

		//set paddle size
		public void set_size(float x, float y) {
			obj.Size = new Vector2f(x, y);
		}
	}
}
