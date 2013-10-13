using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML;
using SFML.Window;
using SFML.Graphics;

namespace SFMLProject
{
	class Game
	{
		public Ball ball;
		public Paddle paddle;
		public RenderWindow window;
		public ArrayList colors = new ArrayList();
		public List<Brick> bricks = new List<Brick>();

		public int mouse_x			= 0; //mouse x position on the screen
		public int max_fps			= 60;
		public bool show_mouse		= false;
		public int window_width		= 800;
		public int window_height	= 800;
		public string window_title	= "Breakout Classic";

		//padding variables
		public int paddle_width		= 100;
		public int paddle_height	= 15;

		public Game() {
		}

		public Game(int width, int height, string title) {
			window_width	= width;
			window_title	= title;
			window_height	= height;
		}

		//initialize game window
		public void Run() {

			//initialize game window
			window = new RenderWindow(new VideoMode((uint)window_width, (uint)window_height), window_title);

			//setup fps
			window.SetFramerateLimit((uint)max_fps);

			//show mouse cursor?
			window.SetMouseCursorVisible(show_mouse);

			//close window event argument
			window.Closed += Close;

			//capture mouse movement
			window.MouseMoved += MouseCapture;

			//setup paddle object
			paddle = new Paddle(paddle_width, paddle_height, Color.White);
			paddle.set_position(0, window_width - 30f);

			//setup ball
			ball = new Ball((uint)window_width, (uint)window_height);

			//setup available brick colors
			colors.Add(Color.Green);
			colors.Add(Color.Magenta);
			colors.Add(Color.Green);
			colors.Add(Color.Yellow);

			//setup bricks
			Setup_Bricks();

			//run loop
			Loop();
		}

		//all the game stuff should be ran in this loop
		public void Loop() {
			while(window.IsOpen()) {
				window.DispatchEvents();
				window.Clear();

				//draw paddle
				paddle.set_position(mouse_x - paddle_width/2, paddle.y);
				if(paddle.display)
					window.Draw(paddle.obj);

				//loop through bricks and draw them
				for(int x = 0; x < 20; x++) {
					if(bricks[x].display == true)
						window.Draw(bricks[x].obj);
				}
				
				//move ball
				ball.move();

				//check ball collission
				ball.brick_collision(ref bricks);
				ball.paddle_collision(paddle.obj);
				ball.window_collision((uint)window_width, (uint)window_height);

				//draw ball on screen if not dead
				if(ball.display)
					window.Draw(ball.obj);

				//check if the game's over
				int count = 0;
				for(int x = 0; x < 20; x++) {
					if(!bricks[x].display) ++count;
				}

				if(!ball.display || count == 20) {
					paddle.display = false;
					for(int x = 0; x < 20; x++) {
						bricks[x].display = false;
					}
				}

				window.Display();
			}
		}

		//setup bricks
		public void Setup_Bricks() {

			//setup & create 20 random bricks w/ random colors
			Color color;
			int rnd_color = 0;
			var rnd_num = new Random(); //used to choose random color from array

			for(int x = 0; x < 20; x++) {
				rnd_color = rnd_num.Next(0,4);
				color = (Color) colors[rnd_color];//choose random color
				bricks.Add(new Brick(color));//add random color
			}

			//setup the brick position on the screen. First brick has 5 spacing before to
			//evenly spread the bricks out on the screen
			int row = 0;
			int start = 0;
			int padding_top = 10;
			float position = 5;

			for(int x = 0; x < 20; x++) {

				//if we hit 5 we start a new row
				if(start == 5) { 
					row++; start = 0; 
				}

				//first brick starts at 5 instead of the brick size + padding amount
				//eg.: 5 vs 110 -> 5 -> 110 -> 220 -> 330 perfect 10 'pixel' spacing
				if(start > 0)
					position += bricks[x].obj.Size.X + 10;
				else
					position = 5;

				//set the position based on row/column
				bricks[x].obj.Position = new Vector2f(position, (row * bricks[x].obj.Size.Y) + (row * padding_top) );

				++start;
			}

		}

		//close window
		public void Close(Object s, EventArgs e) {
			RenderWindow w = (RenderWindow) s;
			w.Close();
		}

		//mouse capture
		public void MouseCapture(Object s, MouseMoveEventArgs e) {

			//current x mouse position 
            mouse_x = e.X;

			//lets make sure the paddle doesn't go off the screen
			//since we're position the mouse in the middle of the paddle we need to 
			//detect the center of the paddle and subtract 50% of the width from calculations
			mouse_x = (mouse_x - paddle_width/2 < 0) ? 50 : 
				(mouse_x + paddle_width/2 > window_width) ? window_width - paddle_width/2 : mouse_x;

		}
	}
}
