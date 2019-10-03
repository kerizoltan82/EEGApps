/*
 * 

public class BTDeviceListAdapter : BaseAdapter {

	private LayoutInflater mInflator;
	private ArrayList<BluetoothDevice> mLeDevices;
	private Context mContext;

	public BTDeviceListAdapter(Context context) {
		super();
		mContext = context;
		mLeDevices = new ArrayList<BluetoothDevice>();
		mInflator = LayoutInflateResources.from(mContext);
		
	}

	public void addDevice(BluetoothDevice device) {
		if (!mLeDevices.contains(device)) {
			mLeDevices.add(device);
		}
	}

	public BluetoothDevice getDevice(int position) {
		return mLeDevices.get(position);
	}

	public void clear() {
		mLeDevices.clear();
	}

	@Override
	public int getCount() {
		return mLeDevices.size();
	}

	@Override
	public Object getItem(int i) {
		return mLeDevices.get(i);
	}

	@Override
	public long getItemId(int i) {
		return i;
	}

	@Override
	public View getView(int i, View view, ViewGroup viewGroup) {

		ViewHolder viewHolder;
		// General ListView optimization code.
		if (view == null) {
			view = mInflatoResources.inflate(Resources.layout.listitem_device, null);
			viewHolder = new ViewHolder();
			viewHoldeResources.img1 = (ImageView) view.FindViewById(Resources.Id.img1);
			viewHoldeResources.img2 = (ImageView) view.FindViewById(Resources.Id.img2);
			viewHoldeResources.deviceName = (TextView) view.FindViewById(Resources.Id.device_name);
			view.setTag(viewHolder);
		} else {
			viewHolder = (ViewHolder) view.getTag();
		}

		BluetoothDevice device = mLeDevices.get(i);
		String deviceName = device.getName();
		String deviceAddress = device.getAddress();
		viewHoldeResources.img2.setVisibility(View.GONE);
		if (deviceName != null && deviceName.length() > 0)
			viewHoldeResources.deviceName.setText(deviceName + ", " + deviceAddress);
		else
			viewHoldeResources.deviceName.setText( "No name, " + deviceAddress);

		return view;
	}
	static class ViewHolder {
		ImageView img1;
		ImageView img2;
		TextView deviceName;
	}
}
*/