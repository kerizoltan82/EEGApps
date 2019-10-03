

using System;
using Android.Views;
using Android.Graphics;
using Android.Content;
using Android.Util;
/*
public class DrawWaveView :  View {
	private static String TAG = "DrawWaveView";
	private Path path;
	public Paint paint = null;
	int VIEW_WIDTH= 0;
	int VIEW_HEIGHT = 0;
	Bitmap cacheBitmap = null;
	Canvas cacheCanvas = null;
	Paint bmpPaint = new Paint();
	private int maxPoint = 0;
	private int currentPoint = 0;
	private int maxValue = 0;
	private int minValue = 0;	
	private float x = 0;
	private float y = 0;
	private float prex = 0;
	private float prey = 0;
	private bool restart = true;
	
	private int mBottom = 0;
	private int mHeight = 0;
	private int mLeft = 0;
	private int mWidth = 0;
	
	private float mPixPerHeight = 0;
	private float mPixPerWidth = 0;
	
	public DrawWaveView(Context context) :base (context)
	{
	}

	public DrawWaveView(Context context, IAttributeSet attrs) :base (context, attrs) {
		
		// TODO Auto-generated constructor stub

		
	}
	public void setValue(int maxPoint, int maxValue, int minValue){
		this.maxPoint = maxPoint;
		this.maxValue = maxValue;
		this.minValue = minValue;
	}
	
	public void initView(){


		mBottom = this.getBottom();
		mWidth = this.getWidth();
		mLeft = this.getLeft();
		mHeight = this.getHeight();
		
		mPixPerHeight = (float)mHeight/(maxValue  - minValue);
		mPixPerWidth =  (float)mWidth/maxPoint ;
		
//		Log.d(TAG,"initView  mWidth= " + mWidth + " , mHeight= " + mHeight  );
//		Log.d(TAG,"initView  mBottom= " + mBottom + " , mLeft= " + mLeft  );
//		Log.d(TAG,"initView  mPixPerHeight= "+ mPixPerHeight +" ,mPixPerWidth=" +mPixPerWidth);
		cacheBitmap = Bitmap.createBitmap(mWidth, mHeight, Config.ARGB_8888);
		cacheCanvas = new Canvas();
		path = new Path();
		cacheCanvas.setBitmap(cacheBitmap);
		
		paint = new Paint(Paint.DITHER_FLAG);
		paint.setColor(ColoResources.GREEN);
		paint.setStyle(Paint.Style.STROKE);
		paint.setStrokeWidth(4);
		
		paint.setAntiAlias(true);
		paint.setDither(true);
		currentPoint =0;
	}
	
	public void clear()
	{
		Paint clearPaint = new Paint();
		clearPaint.setXfermode(new PorterDuffXfermode(Mode.CLEAR));
		cacheCanvas.drawPaint(clearPaint);
		clearPaint.setXfermode(new PorterDuffXfermode(Mode.SRC));
		currentPoint =0;
		path.reset();

		invalidate();
	}
	public bool isReady(){
		return initFlag;
	}
	
	public void updateData(int data){
		if(!initFlag){
			return;
		}
		y = translateData2Y(data);
		x = translatePoint2X(currentPoint);
//		Log.d(TAG,"updateData x: " + x + "   , y: " + y);
		if(currentPoint == 0){
			path.moveTo(x, y);
			currentPoint ++;
			prex = x;
			prey = y;
		}else if(currentPoint == maxPoint){
			cacheCanvas.drawPath(path,paint);
			currentPoint = 0;
		}else{
			path.quadTo(prex,prey,x, y);	
			currentPoint ++;
			prex = x;
			prey = y;
		}
		invalidate();
		if(currentPoint == 0){
			clear();
		}
	}

	private float translateData2Y(int data){
		return (float)mBottom - (data - minValue) *mPixPerHeight ;
	}

	private float translatePoint2X(int point){
		return (float)mLeft + point * mPixPerWidth;
	}
	
    private bool initFlag = false;
	@Override
	protected void onDraw(Canvas canvas) {
		// TODO Auto-generated method stub
		canvas.drawBitmap(cacheBitmap, 0, 0, bmpPaint);
		canvas.drawPath(path, paint);
		//supeResources.onDraw(canvas);
		
	}
	@Override
	protected void onConfigurationChanged(Configuration newConfig) {
		// TODO Auto-generated method stub
		supeResources.onConfigurationChanged(newConfig);
		initFlag = false;
		 Log.d(TAG,"onConfigurationChanged");
	}

	// for rotate screen things
	@Override
	protected void onAttachedToWindow() {
		// TODO Auto-generated method stub
		supeResources.onAttachedToWindow();
		 Log.d(TAG,"onAttachedToWindow");
		 initFlag = false;
	}
	// for rotate screen things
	@Override
	protected void onLayout(bool changed, int left, int top, int right,
			int bottom) {
		// TODO Auto-generated method stub
		supeResources.onLayout(changed, left, top, right, bottom);
		Log.d(TAG,"onLayout");
		 if(!initFlag){
			 initView();
			 initFlag = true;
		 }
	}


}

*/