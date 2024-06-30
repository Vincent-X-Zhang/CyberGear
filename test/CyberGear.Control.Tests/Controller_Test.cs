using CyberGear.Control.Params;
using CyberGear.Control.Protocols;
using FluentAssertions;
using Peak.Can.Basic;

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
			var motor = new Controller(0, 127, PcanChannel.Usb01);
			var actual = motor.GetArbitrationId(CmdMode.SET_MECHANICAL_ZERO);
			actual.Should().Be(0x0600007f);
		}

		/// <summary>
		/// д�����
		/// </summary>
		[Fact]
		public void WriteSingleParam_Greater_NOk()
		{
			var motor = new Controller(0, 127, PcanChannel.Usb01);
			Action action = () => motor.WriteParam(new SpdRefParam(40));
			action.Should().Throw<ArgumentOutOfRangeException>();
		}

		/// <summary>
		/// д�����
		/// </summary>
		[Fact]
		public void WriteSingleParam_Less_NOk()
		{
			var motor = new Controller(0, 127, PcanChannel.Usb01);
			Action action = () => motor.WriteParam(new SpdRefParam(-40));
			action.Should().Throw<ArgumentOutOfRangeException>();
		}
	}
}