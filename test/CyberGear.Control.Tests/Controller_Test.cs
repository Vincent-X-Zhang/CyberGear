using CyberGear.Control.Params;
using FluentAssertions;
using System.Diagnostics;

namespace CyberGear.Control.Tests
{
	public class Controller_Test
	{
		/// <summary>
		/// �����ٲ���
		/// </summary>
		[Fact]
		public void GetArbitrationId_Ok()
		{
			var actual = Controller.GetArbitrationId(CmdMode.SET_MECHANICAL_ZERO, 0, 127);
			actual.Should().Be(0x0600007f);
		}

		/// <summary>
		/// д�����
		/// </summary>
		[Fact]
		public void ValidateParam_Greater_NOk()
		{
			Action action = () => Controller.ValidateParam(new SpdRefParam(40));
			action.Should().Throw<ArgumentOutOfRangeException>();
		}

		/// <summary>
		/// д�����
		/// </summary>
		[Fact]
		public void ValidateParam_Less_NOk()
		{
			Action action = () => Controller.ValidateParam(new SpdRefParam(-40));
			action.Should().Throw<ArgumentOutOfRangeException>();
		}


		/// <summary>
		/// ��ʱ
		/// </summary>
		[Fact]
		public void Timeout()
		{
			const int timeoutMilliseconds = 2000;
			var _mre = new ManualResetEvent(false);
			Action action = () =>
			{
				bool isReplyOK = false;
				var t = new Thread(_ =>
				{
					Thread.Sleep(timeoutMilliseconds);
					// �Ѿ���ɺ�, ��Ҫ��������� mre
					if (!isReplyOK)
						_mre.Set();
				})
				{ IsBackground = true };
				t.Start();

				_mre.Reset();
				_mre.WaitOne();

				// �ȴ��߳���������, û�г�ʱ
				if (t.ThreadState == System.Threading.ThreadState.Running)
				{
					isReplyOK = true;
				}
				// �ȴ��߳̽���, �Ѿ���ʱ
				else
				{
					throw new TimeoutException("reply timeout");
				}
			};
			action.Should().Throw<TimeoutException>();
		}


		/// <summary>
		/// û�г�ʱ
		/// </summary>
		[Fact]
		public void NotTimeout()
		{
			const int timeoutMilliseconds = 2000;
			var _mre = new ManualResetEvent(false);
			var t1 = new Thread(_ => 
			{
				Thread.Sleep(1000);
				_mre.Set();
				Debug.WriteLine("ReplyOK");
			})
			{ IsBackground = true };
			t1.Start();

			Action action = () =>
			{
				bool isReplyOK = false;
				var t = new Thread(_ =>
				{
					Thread.Sleep(timeoutMilliseconds);
					// �Ѿ���ɺ�, ��Ҫ��������� mre
					if (!isReplyOK)
						_mre.Set();
				})
				{ IsBackground = true };
				t.Start();

				_mre.Reset();
				_mre.WaitOne();

                // �ȴ��߳���������, û�г�ʱ
				if (t.ThreadState == (System.Threading.ThreadState.WaitSleepJoin | System.Threading.ThreadState.Background))
				{
					isReplyOK = true;
				}
				// �ȴ��߳̽���, �Ѿ���ʱ
				else
				{
					throw new TimeoutException("reply timeout");
				}
			};
			action.Should().NotThrow<TimeoutException>();
		}
	}
}