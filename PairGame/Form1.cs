using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace PairGame
{
	public partial class Form1 : Form
	{
		// Звуки
		SoundPlayer _soundWin = new SoundPlayer("win.wav");
		SoundPlayer _soundPair = new SoundPlayer("pair.wav");
		SoundPlayer _soundWrong = new SoundPlayer("wrong.wav");

		Label firstClicked = null;

		Label secondClicked = null;

		// Для выбора случайных иконок
		Random random = new Random();

		List<string> icons = new List<string>()
		{
			"!", "!", "N", "N", ",", ",", "k", "k", "@", "@",
			"b", "b", "v", "v", "w", "w", "z", "z", "#", "#",
			"a", "a", "С", "С", "d", "d", "e", "e", "f", "f",
			"F", "F", "h", "h", "i", "i",
		};

		// Назначаем каждую иконку из листа случайным квадратикам
		private void AssignIconsTSquares()
		{
			foreach (Control control in tableLayoutPanel1.Controls)
			{

				if (control is Label iconLabel)
				{
					int randomNumber = random.Next(icons.Count);
					iconLabel.Text = icons[randomNumber];
					iconLabel.ForeColor = iconLabel.BackColor;
					icons.RemoveAt(randomNumber);
				}
			}
		}

		public Form1()
		{
			InitializeComponent();

			AssignIconsTSquares();
		}

		/// <summary>
		/// Every label's Click event is handled by this event handler
		/// </summary>
		/// <param name="sender">The label that was clicked</param>
		/// <param name="e"></param>
		private void Label_Click(object sender, EventArgs e)
		{
			if (timer1.Enabled == true)
			{
				return;
			}


			if (sender as Label != null)
			{
				if ((sender as Label).ForeColor == Color.Black)
				{
					return;
				}

				// Если firstClicked = null, то это первая иконка в паре на которую кликнул игрок,
				// показываем ее
				if (firstClicked == null)
				{
					firstClicked = sender as Label;
					firstClicked.ForeColor = Color.Black;

					return;
				}
			}

			// Если программа дошла до сюда, значит таймер не включен, и firstClicked не null,
			// следовательно это клик по второй иконке пары
			// играем звук и делаем ее черной
			_soundPair.Play();
			secondClicked = sender as Label;
			secondClicked.ForeColor = Color.Black;

			// Проверяем все ли картинки открыты
			CheckForWinner();

			// Если иконки одинаковые, то обнуляем переменные firstClicked и secondClicked, затем ретурн,
			// соответственно до старта таймера не доходит
			if (firstClicked.Text == secondClicked.Text)
			{
				firstClicked = null;
				secondClicked = null;

				return;
			}

			// Если программа дошла до сюда, значит игрок ткнул две разные иконки,
			// играем звук и стартуем таймер
			_soundWrong.Play();
			timer1.Start();
		}

		/// <summary>
		/// This timer is started when the player clicks 
		/// two icons that don't match,
		/// so it counts three quarters of a second 
		/// and then turns itself off and hides both icons
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Timer1_Tick(object sender, EventArgs e)
		{
			// Остановить таймер
			timer1.Stop();

			// Скрыть обе иконки
			firstClicked.ForeColor = firstClicked.BackColor;
			secondClicked.ForeColor = secondClicked.BackColor;

			// Сбрасываем firstClicked и secondClicked, чтобы при следующем клике программа знала что это первый клик по паре
			firstClicked = null;
			secondClicked = null;
		}


		/// <summary>
		/// Check every icon to see if it is matched, by 
		/// comparing its foreground color to its background color. 
		/// If all of the icons are matched, the player wins
		/// </summary>
		private void CheckForWinner()
		{
			foreach (Control control in tableLayoutPanel1.Controls)
			{

				if (control is Label iconLabel)
				{
					if (iconLabel.ForeColor == iconLabel.BackColor)
					{
						return;
					}
				}
			}

			// Если цикл не сделал return, значит все иконки открыты
			_soundWin.Play();
			MessageBox.Show("Вы сопоставили все картинки!", "Поздравляем!");
			Close();
		}
	}
}
