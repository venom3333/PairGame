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
using System.Collections;

namespace PairGame
{
	public partial class MainForm : Form
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
		private void AssignIconsToSquares()
		{
			// Новая коллекция чтобы не затереть первоначальную (для повторной игры)
			List<string> gameIcons = new List<string>(icons);
			foreach (Control control in tableLayoutPanel1.Controls)
			{				
				if (control is Label iconLabel)
				{
					int randomNumber = random.Next(gameIcons.Count);
					iconLabel.Text = gameIcons[randomNumber];
					iconLabel.ForeColor = Color.DarkSlateBlue;
					gameIcons.RemoveAt(randomNumber);
				}
			}
			// Таймер до скрытия иконок в начале
			TimerShowAll.Interval = 5000;
			TimerShowAll.Start();
		}

		public MainForm()
		{
			InitializeComponent();
			tableLayoutPanel1.Visible = false;
		}

		private void Init()
		{
			tableLayoutPanel1.Visible = false;
			buttonStart.Visible = true;
			//AssignIconsToSquares();
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
				if ((sender as Label).ForeColor == Color.DarkSlateBlue)
				{
					return;
				}

				// Если firstClicked = null, то это первая иконка в паре на которую кликнул игрок,
				// показываем ее
				if (firstClicked == null)
				{
					firstClicked = sender as Label;
					firstClicked.ForeColor = Color.DarkSlateBlue;

					return;
				}
			}

			// Если программа дошла до сюда, значит таймер не включен, и firstClicked не null,
			// следовательно это клик по второй иконке пары
			// играем звук и делаем ее черной
			_soundPair.Play();
			secondClicked = sender as Label;
			secondClicked.ForeColor = Color.DarkSlateBlue;

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
			Init();
		}


		// Таймер для показа всех картинок в начале
		private void TimerShowAll_Tick(object sender, EventArgs e)
		{
			// Остановить таймер
			TimerShowAll.Stop();

			// Прячем все картинки
			foreach (Control control in tableLayoutPanel1.Controls)
			{
				if (control is Label iconLabel)
				{
					iconLabel.ForeColor = iconLabel.BackColor;
				}
			}
		}

		private void ButtonStart_Click(object sender, EventArgs e)
		{
			buttonStart.Visible = false;
			tableLayoutPanel1.Visible = true;
			AssignIconsToSquares();
		}
	}
}