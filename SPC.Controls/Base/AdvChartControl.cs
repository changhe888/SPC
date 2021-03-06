﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SPC.Controls.Base
{
    public partial class AdvChartControl : DevExpress.XtraCharts.ChartControl
    {
        private DevExpress.XtraCharts.Axis2D _targetAxis = null;
        private DevExpress.XtraCharts.Axis2D targetAxis
        {
            get
            {
                return this._targetAxis;
            }
            set
            {
                if (value == null)
                    this.Cursor = System.Windows.Forms.Cursors.Arrow;
                else
                    this.Cursor = System.Windows.Forms.Cursors.Hand;
                this._targetAxis = value;
            }
        }
        private DevExpress.XtraCharts.ConstantLine _targetLine = null;
        private DevExpress.XtraCharts.ConstantLine targetLine
        {
            get
            {
                return this._targetLine;
            }
            set
            {
                if (value == null)
                    this.Cursor = System.Windows.Forms.Cursors.Arrow;
                else if (this.Diagram is DevExpress.XtraCharts.XYDiagram&&(this.Diagram as DevExpress.XtraCharts.XYDiagram).Rotated)
                {
                    if (value.Name == "X")
                        this.Cursor = System.Windows.Forms.Cursors.SizeNS;
                    else if (value.Name == "Y")
                        this.Cursor = System.Windows.Forms.Cursors.SizeWE;
                }
                else
                {
                    if (value.Name == "X")
                        this.Cursor = System.Windows.Forms.Cursors.SizeWE;
                    else if (value.Name == "Y")
                        this.Cursor = System.Windows.Forms.Cursors.SizeNS;
                }
                this._targetLine = value;
            }
        }
        private DevExpress.XtraBars.PopupMenu RightClickPopupMenu = new DevExpress.XtraBars.PopupMenu();
        private DevExpress.XtraBars.BarStaticItem popupMenuStaticItem = new DevExpress.XtraBars.BarStaticItem();
        private DevExpress.XtraBars.BarEditItem popupMenuEditItem = new DevExpress.XtraBars.BarEditItem();
        private DevExpress.XtraBars.BarButtonItem popupMenuDeleteButtonItem = new DevExpress.XtraBars.BarButtonItem();
        private DevExpress.XtraBars.BarButtonItem popupMenuXAddButtonItem = new DevExpress.XtraBars.BarButtonItem() { Caption = "添加X轴边界" };
        private DevExpress.XtraBars.BarButtonItem popupMenuYAddButtonItem = new DevExpress.XtraBars.BarButtonItem() { Caption = "添加Y轴边界" };
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit popupMenuTextEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
        private DevExpress.XtraBars.BarManager mybarmanager = new DevExpress.XtraBars.BarManager();
        public event EventHandler<ShowRightClickPopupMenuEventArgs> CustomShowRightClickPopupMenu;
        public class ShowRightClickPopupMenuEventArgs : EventArgs
        {
            public DevExpress.XtraBars.PopupMenu RightClickPopupMenu { get; private set; }
            public DevExpress.XtraCharts.ConstantLine ConstantLine { get; private set; }
            public bool Handle;
            public ShowRightClickPopupMenuEventArgs(DevExpress.XtraBars.PopupMenu menu, DevExpress.XtraCharts.ConstantLine line, bool handle)
            {
                this.RightClickPopupMenu = menu;
                this.ConstantLine = line;
                this.Handle = handle;
            }
        }
        public AdvChartControl()
        {
            InitializeComponent();
            InitPopupMenu();
            popupMenuDeleteButtonItem.ItemClick += popupMenuDeleteButtonItem_ItemClick;
            popupMenuEditItem.EditValueChanged += popupMenuEditItem_EditValueChanged;
            popupMenuXAddButtonItem.ItemClick += popupMenuXAddButtonItem_ItemClick;
            popupMenuYAddButtonItem.ItemClick += popupMenuYAddButtonItem_ItemClick;
            this.RuntimeHitTesting = true;
            this.RightClickPopupMenu.Manager = mybarmanager;
            this.popupMenuEditItem.Edit = this.popupMenuTextEdit;
            this.mybarmanager.Form = this;
        }

        private void popupMenuYAddButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var line = new DevExpress.XtraCharts.ConstantLine("Y", (e.Item.Tag as object[])[0].ToString());
            ((e.Item.Tag as object[])[1] as DevExpress.XtraCharts.Axis2D).ConstantLines.Add(line);
            line.Title.Text = String.Format("{0:N3}", line.AxisValue);
        }

        private void popupMenuXAddButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var line = new DevExpress.XtraCharts.ConstantLine("X", (e.Item.Tag as object[])[0].ToString());
            ((e.Item.Tag as object[])[1] as DevExpress.XtraCharts.Axis2D).ConstantLines.Add(line);
            line.Title.Text = String.Format("{0:N3}", line.AxisValue); ;
        }
        private void popupMenuEditItem_EditValueChanged(object sender, EventArgs e)
        {
            var t = ((sender as DevExpress.XtraBars.BarEditItem).Tag as DevExpress.XtraCharts.ConstantLine);
            string s = (sender as DevExpress.XtraBars.BarEditItem).EditValue.ToString();
            if (t != null)
            {
                t.AxisValue = s;
                t.Title.Text = String.Format("{0:N3}", t.AxisValue);
            }
        }
        private void InitPopupMenu()
        {
            this.RightClickPopupMenu.ClearLinks();
        }
        private void popupMenuDeleteButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var line = (e.Item.Tag as DevExpress.XtraCharts.ConstantLine);
            foreach(var axis in (this.Diagram as DevExpress.XtraCharts.XYDiagram2D).GetAllAxesX().Union((this.Diagram as DevExpress.XtraCharts.XYDiagram2D).GetAllAxesY()))
            {
                axis.ConstantLines.Remove(line);
            }
        }
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (!DesignMode)
            {
                var info = this.CalcHitInfo(e.Location);
                if (info.Diagram != null)
                {

                    if (e.Button == System.Windows.Forms.MouseButtons.Left && info.InConstantLine)
                    {
                        this.targetLine = info.ConstantLine;
                    }
                    else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        var pointinfo = (this.Diagram as DevExpress.XtraCharts.XYDiagram2D).PointToDiagram(e.Location);
                        InitPopupMenu();
                        object argument;
                        if (pointinfo.ArgumentScaleType == DevExpress.XtraCharts.ScaleType.Numerical)
                            argument = pointinfo.NumericalArgument;
                        else
                            argument = pointinfo.QualitativeArgument;
                        popupMenuStaticItem.Caption = string.Format("X:{0} Y:{1:N3}", argument, pointinfo.NumericalValue);
                        this.RightClickPopupMenu.AddItem(popupMenuStaticItem);
                        popupMenuXAddButtonItem.Tag = new object[] { argument, pointinfo.AxisX };
                        this.RightClickPopupMenu.AddItem(popupMenuXAddButtonItem);
                        ShowRightClickPopupMenuEventArgs eventarg;
                        if (info.InConstantLine)
                        {
                            popupMenuEditItem.BeginUpdate();
                            popupMenuEditItem.Caption = info.ConstantLine.Name + "轴边界线";
                            popupMenuEditItem.Tag = info.ConstantLine;
                            popupMenuEditItem.EditValue = info.ConstantLine.AxisValue;
                            popupMenuEditItem.EndUpdate();
                            this.RightClickPopupMenu.AddItem(popupMenuEditItem);
                            popupMenuDeleteButtonItem.Tag = info.ConstantLine;
                            popupMenuDeleteButtonItem.Caption = "删除" + info.ConstantLine.Name + "轴边界线";
                            this.RightClickPopupMenu.AddItem(popupMenuDeleteButtonItem);

                        }
                        eventarg = new ShowRightClickPopupMenuEventArgs(RightClickPopupMenu, info.ConstantLine, true);
                        if (CustomShowRightClickPopupMenu != null)
                            CustomShowRightClickPopupMenu(RightClickPopupMenu, eventarg);
                        if (eventarg.Handle)
                            this.RightClickPopupMenu.ShowPopup(MousePosition);
                    }
                }
                else if (info.Axis != null && (this.Diagram as DevExpress.XtraCharts.XYDiagram2D).GetAllAxesY().Contains(info.Axis))
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        this.targetAxis = (info.Axis as DevExpress.XtraCharts.Axis2D);
                    }
                    else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        var pointinfo = (this.Diagram as DevExpress.XtraCharts.XYDiagram2D).PointToDiagram(new System.Drawing.Point(this.Width / 2, e.Location.Y));
                        InitPopupMenu();
                        var value = pointinfo.GetAxisValue(info.Axis).NumericalValue;
                        popupMenuStaticItem.Caption = string.Format("X: Y:{0:N3}", value);
                        this.RightClickPopupMenu.AddItem(popupMenuStaticItem);
                        popupMenuYAddButtonItem.Tag = new object[] { value, info.Axis };
                        this.RightClickPopupMenu.AddItem(popupMenuYAddButtonItem);
                        ShowRightClickPopupMenuEventArgs eventarg;
                        eventarg = new ShowRightClickPopupMenuEventArgs(RightClickPopupMenu, info.ConstantLine, true);
                        if (CustomShowRightClickPopupMenu != null)
                            CustomShowRightClickPopupMenu(RightClickPopupMenu, eventarg);
                        if (eventarg.Handle)
                            this.RightClickPopupMenu.ShowPopup(MousePosition);
                    }

                }
            }
        }
        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (!DesignMode)
            {
                if (this.targetLine != null)
                {
                    var info = (this.Diagram as DevExpress.XtraCharts.XYDiagram2D).PointToDiagram(e.Location);
                    if (targetLine.Name == "Y" && !info.IsEmpty)
                    {
                        targetLine.AxisValue = info.NumericalValue;
                        targetLine.Title.Text = String.Format("{0:N3}", targetLine.AxisValue);
                    }
                    else if (targetLine.Name == "X" && !info.IsEmpty)
                    {
                        object argument;
                        if (info.ArgumentScaleType == DevExpress.XtraCharts.ScaleType.Qualitative)
                            argument = info.QualitativeArgument;
                        else
                            argument = info.NumericalArgument;
                        targetLine.AxisValue = argument;
                        targetLine.Title.Text = String.Format("{0:N3}", targetLine.AxisValue);
                    }
                }
                else if (this.targetAxis != null)
                {
                    var info = this.CalcHitInfo(e.Location);
                    if (info.Axis != null && (this.Diagram as DevExpress.XtraCharts.XYDiagram2D).GetAllAxesY().Contains(info.Axis))
                    {
                        DevExpress.XtraCharts.XYDiagramSeriesViewBase sv;
                        DevExpress.XtraCharts.SwiftPlotSeriesViewBase spv;
                        foreach (DevExpress.XtraCharts.Series s in this.Series)
                        {
                            if ((sv = s.View as DevExpress.XtraCharts.XYDiagramSeriesViewBase) != null && sv.AxisY == targetAxis &&(info.Axis as DevExpress.XtraCharts.AxisY).GetVisibilityInPane(sv.Pane))
                                sv.AxisY = info.Axis as DevExpress.XtraCharts.AxisY;
                            else if ((spv = s.View as DevExpress.XtraCharts.SwiftPlotSeriesViewBase) != null && spv.AxisY == targetAxis && (info.Axis as DevExpress.XtraCharts.SwiftPlotDiagramAxisYBase).GetVisibilityInPane(spv.Pane))
                                spv.AxisY = info.Axis as DevExpress.XtraCharts.SwiftPlotDiagramAxisYBase;
                        }
                    }
                    else if (e.Location.X > this.Width / 2)
                        this.targetAxis.Alignment = DevExpress.XtraCharts.AxisAlignment.Far;
                    else
                        this.targetAxis.Alignment = DevExpress.XtraCharts.AxisAlignment.Near;
                }
                this.targetLine = null;
                this.targetAxis = null;
            }
        }
        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!DesignMode)
            {
                if (this.targetLine == null&&this.targetAxis==null)
                    this.Cursor = System.Windows.Forms.Cursors.Arrow;
                if (this.Series.Count > 0)
                {
                    var info = this.CalcHitInfo(e.Location);
                    if (info.ConstantLine != null)
                    {
                        if (this.Diagram is DevExpress.XtraCharts.XYDiagram&&(this.Diagram as DevExpress.XtraCharts.XYDiagram).Rotated)
                        {
                                                        if (info.ConstantLine.Name == "Y")
                                this.Cursor = System.Windows.Forms.Cursors.SizeWE;
                            else if (info.ConstantLine.Name == "X")
                                this.Cursor = System.Windows.Forms.Cursors.SizeNS;
                        }
                        else
                        {
                            if (info.ConstantLine.Name == "Y")
                                this.Cursor = System.Windows.Forms.Cursors.SizeNS;
                            else if (info.ConstantLine.Name == "X")
                                this.Cursor = System.Windows.Forms.Cursors.SizeWE;
                        }
                    }
                }
                if (this.targetLine != null)
                {
                    var info = (this.Diagram as DevExpress.XtraCharts.XYDiagram2D).PointToDiagram(e.Location);
                    if (targetLine.Name == "Y" && !info.IsEmpty)
                    {
                        targetLine.AxisValue = info.NumericalValue;
                        targetLine.Title.Text = String.Format("{0:N3}", targetLine.AxisValue);
                    }
                    else if (targetLine.Name == "X" && !info.IsEmpty)
                    {
                        object argument;
                        if (info.ArgumentScaleType == DevExpress.XtraCharts.ScaleType.Qualitative)
                            argument = info.QualitativeArgument;
                        else
                            argument = info.NumericalArgument;
                        targetLine.AxisValue = argument;
                        targetLine.Title.Text = String.Format("{0:N3}", targetLine.AxisValue);
                    }

                }

            }
        }
    }
    //public static class AddExtention
    //{
    //    public static int Add(this DevExpress.XtraCharts.ConstantLineCollection col,DevExpress.XtraCharts.ConstantLine target)
    //    {
    //        int result;
    //        if(col.Count>0&&(col[col.Count].Name=="X"))
    //        {
    //            DevExpress.XtraCharts.ConstantLine[] temp = new DevExpress.XtraCharts.ConstantLine[] { col[col.Count - 1], col[col.Count] };
    //            col.RemoveAt(col.Count);
    //            col.RemoveAt(col.Count - 1);
    //            result = col.Add(target);
    //            col.AddRange(temp);
    //        }
    //        else
    //            result = col.Add(target);
    //        return result;
    //    }
    //}
}
