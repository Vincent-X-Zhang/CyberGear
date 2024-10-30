namespace CyberGear.Control
{
	/// <summary>
	/// can ������
	/// </summary>
	public class CanBusBuilder
	{
		/// <summary>
		/// can ����
		/// </summary>
		public readonly CanbusOption CanbusOption;
		/// <summary>
		/// �������
		/// </summary>
		public readonly SlotType SlotType;
		/// <summary>
		/// ������
		/// </summary>
		public readonly int SlotIndex;

		/// <summary>
		/// can ������
		/// </summary>
		/// <param name="slotType">�������</param>
		/// <param name="slotIndex">������</param>
		public CanBusBuilder(SlotType slotType, int slotIndex)
		{
			CanbusOption = new CanbusOption();
			SlotType = slotType;
			SlotIndex = slotIndex;
		}

		public void Configure(Action<CanbusOption> method)
		{
			method(CanbusOption);
		}

		public CanBus Build()
			=> new CanBus(this);
	}
}