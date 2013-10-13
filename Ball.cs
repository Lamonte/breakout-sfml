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
    class Ball
    {
		public float x;
		public float y;
        public int speed_y = 5;
		public int speed_x = 5;
        public CircleShape obj;
		public int direction = 0; //0 = not moving left/right, -1 left, 1 = right
        public bool falling = true; //should be set to false once the ball hits the paddle for the first time?
		public bool display = true;
		public bool hitpaddle = false;
        
        public Ball(uint winw, uint winh) {

            //setup ball object
            obj = new CircleShape(5f);
            obj.FillColor = Color.White;
            obj.Position = new Vector2f(winw/2 - 5f, winh - 400f);

			x = obj.Position.X;
			y = obj.Position.Y;

            //setup the random drop direction
            Random rng = new Random();
            int num = rng.Next(1, 3);

			//set drop direction
            if(num == 1) direction = -1;
            if(num == 2) direction = 1;

        }

        public void move() {
            
            //check if the balls falling (start of game) so we can
			//set the starting direction
            if(falling) {
                if(direction < 0) speed_x = -speed_x;
				falling = false;
            }

			//ball movement should be standard here by incrementing x & y
			//by its movement speed speed; hitting the paddle will change its
			//direction
			if(!falling) {
				x += speed_x;
				y += speed_y;
			}

			//update the ball position based on the new position
			obj.Position = new Vector2f(x, y);

			x = obj.Position.X;
			y = obj.Position.Y;
        }

		public void brick_collision(ref List<Brick> bricks) {
			
			float brickx = 0;
			float bricky = 0;
			
			int rng = 0;
			Random rnd = new Random();

			//loop through the 20 bricks in the array
			for(int pos = 0; pos < 20; pos++) {
				
				brickx = bricks[pos].obj.Position.X;
				bricky = bricks[pos].obj.Position.Y;

				//check if the ball touches any of the bricks and change the Y direction
				//not the X direction
				if(x > brickx && x + obj.Radius < brickx + bricks[pos].obj.Size.X
					&& y + obj.Radius > bricky && y < bricky + bricks[pos].obj.Size.Y && bricks[pos].display == true) {
					speed_y = -speed_y;

					bricks[pos].display = false;
				}
			}
		}

		//paddle collision
		public void paddle_collision(RectangleShape paddle) {
			
			float paddlex = paddle.Position.X;
			float paddley = paddle.Position.Y;

			//check if ball hits paddle
			if(x > paddlex && x + obj.Radius < paddlex + paddle.Size.X
				&& y + obj.Radius > paddley) { 

				//when paddle hits the ball we want to slightly increase or decrease the speed
				//randomly so we can slightly change the angle of the ball so it doesn't get
				//stuck going into the same direction making it impossible to complete the game
				Random rnd = new Random();
				int rng = rnd.Next(2,7);

				//make sure the speed isn't the same as the previous speed because of the above ^
				while(rng == speed_x) {
					rng = rnd.Next(2,7);
				}

				//change the direction of the ball based on which side it hits
				//this is the left side, if it hits the left side the ball will go left else right
				if(x > paddlex && x + obj.Radius < paddlex + paddle.Size.X - (paddle.Size.X/2)
					&& y + obj.Radius > paddley) {
					speed_x = -rng;
				} else {
					speed_x = rng;
				}

				//if the ball hits the paddle make it go in the opposite direction (up instead of down)
				speed_y = -speed_y;

			}
		}

		//detect window collision 
		public void window_collision(uint width, uint height) {

			//ball is going left and hits the left wall redirect it the opposite way
			if(x < 0)
				speed_x = -speed_x;

			//if ball hits right wall redirect it to the left
			if(x + obj.Radius > width)
				speed_x = -speed_x;

			//if ball hits the top wall redirect it down
			if(y < 0)
				speed_y = -speed_y;

			//we hit the bottom of the window game over
			if(y + obj.Radius > height)
				display = false;
		}

    }
}
