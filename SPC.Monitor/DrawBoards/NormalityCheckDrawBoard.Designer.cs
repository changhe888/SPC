﻿namespace SPC.Monitor.DrawBoards
{
    partial class NormalityCheckDrawBoard
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.PointSeriesView pointSeriesView1 = new DevExpress.XtraCharts.PointSeriesView();
            DevExpress.XtraCharts.RegressionLine regressionLine1 = new DevExpress.XtraCharts.RegressionLine();
            DevExpress.XtraCharts.PointSeriesView pointSeriesView2 = new DevExpress.XtraCharts.PointSeriesView();
            this.chartControl1 = new SPC.Controls.Base.AdvChartControl();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(regressionLine1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesView2)).BeginInit();
            this.SuspendLayout();
            // 
            // chartControl1
            // 
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram1.EnableAxisXScrolling = true;
            xyDiagram1.EnableAxisYScrolling = true;
            xyDiagram1.ScrollingOptions.UseKeyboard = false;
            xyDiagram1.ScrollingOptions.UseMouse = false;
            xyDiagram1.ScrollingOptions.UseTouchDevice = false;
            xyDiagram1.ZoomingOptions.UseMouseWheel = false;
            this.chartControl1.Diagram = xyDiagram1;
            this.chartControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartControl1.Legend.Visible = false;
            this.chartControl1.Location = new System.Drawing.Point(0, 0);
            this.chartControl1.Name = "chartControl1";
            this.chartControl1.RuntimeHitTesting = true;
            series1.Name = "Series 1";
            regressionLine1.Name = "Regression Line 1";
            pointSeriesView1.Indicators.AddRange(new DevExpress.XtraCharts.Indicator[] {
            regressionLine1});
            pointSeriesView1.PointMarkerOptions.Size = 3;
            series1.View = pointSeriesView1;
            this.chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.chartControl1.SeriesTemplate.View = pointSeriesView2;
            this.chartControl1.Size = new System.Drawing.Size(747, 360);
            this.chartControl1.TabIndex = 0;
            // 
            // NormalityCheckDrawBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chartControl1);
            this.Name = "NormalityCheckDrawBoard";
            this.Size = new System.Drawing.Size(747, 360);
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(regressionLine1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.Base.AdvChartControl chartControl1;
    }
}
