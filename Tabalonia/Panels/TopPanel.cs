namespace Tabalonia.Panels;

public class TopPanel : Panel
{
    private Layoutable TabsControl => Children[0];
    private Layoutable AddTabButton => Children[1];

    protected override Size MeasureOverride(Size availableSize)
    {
        double height = 0;
        double width = 0;

        if (Children.Count != 2)
            return new Size(width, height);

        double availableWidth = availableSize.Width;
        double availableHeight = availableSize.Height;

        MeasureControl(AddTabButton, ref width, ref availableWidth, availableHeight);

        TabsControl.Measure(new Size(availableWidth, availableHeight));

        width += TabsControl.DesiredSize.Width;

        height = Math.Max(TabsControl.DesiredSize.Height, AddTabButton.DesiredSize.Height);

        return new Size(width, height);

        static void MeasureControl(Layoutable control, ref double w, ref double aW, in double h)
        {
            control.Measure(new Size(aW, h));
            w += control.DesiredSize.Width;
            aW -= control.DesiredSize.Width;
        }
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        if (Children.Count != 2)
            return finalSize;

        const double leftThumbWidth = 0;
        double tabsWidth = TabsControl.DesiredSize.Width;
        double addTabButtonWidth = AddTabButton.DesiredSize.Width;

        double tabsHeight = Math.Max(TabsControl.DesiredSize.Height, finalSize.Height);

        double withoutTabsWidth = leftThumbWidth + addTabButtonWidth;
        double availableTabsWidth = finalSize.Width - withoutTabsWidth;

        //|                         finalSize.Width                            |
        //
        //   if (tabsWidth < availableTabsWidth):
        //|leftThumb|tab1    |tab2    |addTabButton|         rightThumb        |
        //
        //   else
        //|leftThumb|tab1|tab2|tab3|tab4|tab5|tab6|tab7|addTabButton|rightThumb|

        if (tabsWidth < availableTabsWidth)
        {
            ArrangeWhenTabsFit(x: leftThumbWidth, tabsWidth, tabsHeight);
            return finalSize;
        }

        ArrangeWhenTabsUnfit(x: leftThumbWidth, tabsHeight, availableTabsWidth);
        return finalSize;
    }

    /// <summary>
    /// |leftThumb|tab1    |tab2    |addTabButton|         rightThumb        |
    /// </summary>
    private void ArrangeWhenTabsFit(double x, in double tabsWidth, in double tabsHeight)
    {
        TabsControl.Arrange(new Rect(x, 0, tabsWidth, tabsHeight));
        x += tabsWidth;

        ArrangeCenterVertical(AddTabButton, x, tabsHeight);
    }

    /// <summary>
    /// |leftThumb|tab1|tab2|tab3|tab4|tab5|tab6|tab7|addTabButton|rightThumb|
    /// </summary>
    private void ArrangeWhenTabsUnfit(double x, in double tabsHeight, in double availableTabsWidth)
    {
        TabsControl.Arrange(new Rect(x, 0, availableTabsWidth, tabsHeight));

        x += availableTabsWidth;

        ArrangeCenterVertical(AddTabButton, x, tabsHeight);
    }

    private static void ArrangeCenterVertical(Layoutable control, double x, double fullHeight)
    {
        double width = control.DesiredSize.Width;
        double height = control.DesiredSize.Height;

        double y = (fullHeight - height) / 2;

        control.Arrange(new Rect(x, y, width, height));
    }
}