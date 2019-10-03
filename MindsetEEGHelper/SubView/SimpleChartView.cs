using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Util;

namespace gnf
{

    [Register("hu/kerizoltan/mindseteeg/SimpleChartView")]
    public class SimpleChartView : View
    {

        int chartLineWidth = 3;
        int textSize = 40;

        public SimpleChartView(Context context, IAttributeSet attrs) : base(context, attrs) {
            SetWillNotDraw(false);

            chartLineWidth = Resources.GetDimensionPixelSize(Resource.Dimension.ext_chart_line_width);
            textSize = Resources.GetDimensionPixelSize(Resource.Dimension.chart_font_size);

            linePaintGrey = new Paint(PaintFlags.AntiAlias);
            linePaintGrey.Color = new Color(140, 140, 140);
            linePaintGrey.StrokeWidth = chartLineWidth;
            linePaintGrey.TextSize = 34;


            linePaintX = new Paint(PaintFlags.AntiAlias);
            linePaintX.Color = Color.BlueViolet;
            linePaintX.StrokeWidth = chartLineWidth*2;
            linePaintX.TextSize = 42;
            SetPaint(linePaintX, true);
            //SetShader(linePaintX);

            linePaintX2 = new Paint(PaintFlags.AntiAlias);
            linePaintX2.Color = Color.Red;
            linePaintX2.StrokeWidth = chartLineWidth*2;
            linePaintX2.TextSize = 42;
            SetPaint(linePaintX2, true);

            linePaints.Add(linePaintX);
            linePaints.Add(linePaintX2);

            //default
            bgPaint = new Paint(PaintFlags.AntiAlias);
            bgPaint.Color = new Color(0, 0, 0);
            bgPaint.StrokeWidth = chartLineWidth;
        }

        Paint bgPaint;
        Paint linePaintGrey;
        Paint linePaintX;
        Paint linePaintX2;

        List<Paint> linePaints = new List<Paint>();

        public List<double[]> Values = new List<double[]>();
        public List<double[]> Values2 = new List<double[]>();


        void SetPaint(Paint apaint, bool HasRoundedPath) {

            var cornerPathEffectSize = this.Resources.GetDimensionPixelSize(Resource.Dimension.corner_path_effect_size_simple);

                apaint.StrokeJoin = Paint.Join.Round;
                apaint.StrokeCap = Paint.Cap.Round;
                apaint.SetStyle(Paint.Style.Stroke);
                apaint.AntiAlias = true;
                if(HasRoundedPath) {
                    apaint.SetPathEffect(new CornerPathEffect(cornerPathEffectSize));
                }
                // do this only once, do not change IsDashed later!
                /*
                 * else if(IsDashed) {
                    apaint.SetPathEffect(new DashPathEffect(new float[] { groupSignalWidth * 1.2f, groupSignalWidth * 1.2f, 20, 20 }, 0));
                    apaint.SetStyle(Paint.Style.Stroke);
                }
                */
            
        }

        protected void SetShader(Paint apaint) {
            //Shader shader = new LinearGradient(0, shaderyOffset, 0, bottom - shaderyOffset, CrossStyleGuide.ChartLineGradientStartColor.ToAndroidColor(), CrossStyleGuide.ChartLineGradientEndColor.ToAndroidColor(), Shader.TileMode.Clamp);
            //apaint.SetShader(shader);
        }

        protected override void OnDraw(Canvas canvas) {
            // draw the background as usual
            base.OnDraw(canvas);

            if( Values.Count == 0) return;

            try {

                var margins = (ViewGroup.MarginLayoutParams)LayoutParameters;
                int left = margins.LeftMargin;
                int top = margins.TopMargin;
                int bottom = Height - margins.BottomMargin;
                int right = Width - margins.RightMargin;
                int x_scale = Width;
                int y_scale = Height-10;

                //canvas.DrawRect(left, top, right, bottom, bgPaint);

                for(int i = 1;i < Values.Count;i++) {
                    int xstart = (int) (x_scale * Values[i-1][0]);
                    int ystart = 5+ (int)(y_scale * Values[i-1][1]);
                    int xend = (int)(x_scale * Values[i][0]);
                    int yend = 5+ (int)(y_scale * Values[i][1]);
                    //canvas.DrawLine(xstart, ystart, xend, yend, linePaintX);

                    var path = GetLinePath(xstart, ystart, xend, yend);
                    canvas.DrawPath(path, linePaints[0]);
                }

                if(Values2 != null) {
                    for(int i = 1;i < Values2.Count;i++) {
                        int xstart = (int)(x_scale * Values2[i - 1][0]);
                        int ystart = 5 + (int)(y_scale * Values2[i - 1][1]);
                        int xend = (int)(x_scale * Values2[i][0]);
                        int yend = 5 + (int)(y_scale * Values2[i][1]);
                        //canvas.DrawLine(xstart, ystart, xend, yend, linePaintX);

                        var path = GetLinePath(xstart, ystart, xend, yend);
                        canvas.DrawPath(path, linePaints[1]);
                    }
                }
              

            } catch(Exception ex) {
               System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        Path GetLinePath(int xstart, int ystart, int xend, int yend) {
            Path path = new Path();
            path.MoveTo(xstart, ystart);
            path.LineTo(xend, yend);
            return path;
        }

    }

}