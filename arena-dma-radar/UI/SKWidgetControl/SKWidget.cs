using eft_dma_shared.Common.Misc;

namespace arena_dma_radar.UI.SKWidgetControl
{
    public abstract class SKWidget : IDisposable
    {
        #region Fields
        private readonly Lock _sync = new();
        private readonly SKGLControl _parent;
        private bool _titleDrag = false;
        private bool _resizeDrag = false;
        private Point _lastMousePosition;
        private SKPoint _location = new(1, 1);
        private SKSize _size = new(200, 200);
        private SKPath _resizeTriangle;
        private float _relativeX;
        private float _relativeY;
        #endregion

        #region Private Properties
        private float TitleBarHeight => 12.5f * ScaleFactor;
        private SKRect TitleBar => new(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Top + TitleBarHeight);
        private SKRect MinimizeButton => new(TitleBar.Right - TitleBarHeight,
            TitleBar.Top, TitleBar.Right, TitleBar.Bottom);
        #endregion

        #region Protected Properties
        protected string Title { get; }
        protected bool CanResize { get; }
        protected float ScaleFactor { get; private set; }
        protected SKPath ResizeTriangle => _resizeTriangle;
        #endregion

        #region Public Properties
        public bool Minimized { get; protected set; }
        public SKRect ClientRectangle => new(Rectangle.Left, Rectangle.Top + TitleBarHeight, Rectangle.Right, Rectangle.Bottom);
        public SKSize Size
        {
            get => _size;
            set
            {
                lock (_sync)
                {
                    if (!value.Width.IsNormalOrZero() || !value.Height.IsNormalOrZero())
                        return;
                    if (value.Width < 0f || value.Height < 0f)
                        return;
                    value.Width = (int)value.Width;
                    value.Height = (int)value.Height;
                    _size = value;
                    InitializeResizeTriangle();
                }
            }
        }
        public SKPoint Location
        {
            get => _location;
            set
            {
                lock (_sync)
                {
                    if (value.X != 0f && !value.X.IsNormalOrZero() ||
                        value.Y != 0f && !value.Y.IsNormalOrZero())
                        return;
                    var cr = _parent.ClientRectangle;
                    if (cr.Width == 0 ||
                        cr.Height == 0)
                        return;
                    _location = value;
                    CorrectLocationBounds(cr);
                    _relativeX = value.X / cr.Width;
                    _relativeY = value.Y / cr.Height;
                    InitializeResizeTriangle();
                }
            }
        }
        public SKRect Rectangle => new SKRect(Location.X,
            Location.Y,
            Location.X + Size.Width,
            Location.Y + Size.Height + TitleBarHeight);
        #endregion

        #region Constructor
        protected SKWidget(SKGLControl parent, string title, SKPoint location, SKSize clientSize, float scaleFactor, bool canResize = true)
        {
            _parent = parent;
            CanResize = canResize;
            Title = title;
            ScaleFactor = scaleFactor;
            Size = clientSize;
            Location = location;
            parent.MouseClick += Parent_MouseClick;
            parent.MouseDown += Parent_MouseDown;
            parent.MouseUp += Parent_MouseUp;
            parent.MouseMove += Parent_MouseMove;
            parent.Resize += Parent_Resize;
            InitializeResizeTriangle();
            CanResize = canResize;
        }

        private void Parent_Resize(object sender, EventArgs e)
        {
            var parentBounds = _parent.ClientRectangle;

            // Calculate the new location based on relative position
            Location = new SKPoint(
                parentBounds.Width * _relativeX,
                parentBounds.Height * _relativeY);
        }
        #endregion

        #region Hooked Parent Events
        private void Parent_MouseMove(object sender, MouseEventArgs e)
        {
            if (_resizeDrag && CanResize)
            {
                if (e.X < Rectangle.Left || e.Y < Rectangle.Top)
                    return;
                var newSize = new SKSize(Math.Abs(Rectangle.Left - e.X), Math.Abs(Rectangle.Top - e.Y));
                Size = newSize;
            }
            else if (_titleDrag)
            {
                int deltaX = e.X - _lastMousePosition.X;
                int deltaY = e.Y - _lastMousePosition.Y;
                var newLoc = new SKPoint(Location.X + deltaX, Location.Y + deltaY);
                // Set the new location
                Location = newLoc;
            }
            _lastMousePosition = new(e.X, e.Y);
        }

        private void Parent_MouseUp(object sender, MouseEventArgs e)
        {
            _titleDrag = false;
            _resizeDrag = false;
        }

        private void Parent_MouseDown(object sender, MouseEventArgs e)
        {
            _lastMousePosition = new(e.X, e.Y);
            var test = HitTest(new SKPoint(e.X, e.Y));
            switch (test)
            {
                case WidgetClickEvent.ClickedTitleBar:
                    _titleDrag = true;
                    break;
                case WidgetClickEvent.ClickedResize:
                    _resizeDrag = true;
                    break;
                default:
                    break;
            }
        }

        private void Parent_MouseClick(object sender, MouseEventArgs e)
        {
            var test = HitTest(new SKPoint(e.X, e.Y));
            switch (test)
            {
                case WidgetClickEvent.ClickedMinimize:
                    Minimized = !Minimized;
                    Location = Location;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Public Methods
        public virtual void Draw(SKCanvas canvas)
        {
            if (!Minimized)
                canvas.DrawRect(Rectangle, WidgetBackgroundPaint);
            canvas.DrawRect(TitleBar, TitleBarPaint);
            float titleCenterY = TitleBar.Top + TitleBar.Height / 2;
            float titleYOffset = (TitleBarText.FontMetrics.Ascent + TitleBarText.FontMetrics.Descent) / 2;
            canvas.DrawText(Title,
                new(TitleBar.Left + 2.5f * ScaleFactor,
                titleCenterY - titleYOffset),
                TitleBarText);
            canvas.DrawRect(MinimizeButton, ButtonBackgroundPaint);
            // Draw Rest of stuff...
            DrawMinimizeButton(canvas);
            if (!Minimized && CanResize)
                DrawResizeCorner(canvas);
        }

        public virtual void SetScaleFactor(float newScale)
        {
            ScaleFactor = newScale;
            InitializeResizeTriangle();
        }
        #endregion

        #region Private Methods
        private void CorrectLocationBounds(Rectangle clientRectangle)
        {
            var rect = Minimized ? TitleBar : Rectangle;

            if (rect.Left < clientRectangle.Left)
                _location = new SKPoint(clientRectangle.Left, _location.Y);
            else if (rect.Right > clientRectangle.Right)
                _location = new SKPoint(clientRectangle.Right - rect.Width, _location.Y);

            if (rect.Top < clientRectangle.Top)
                _location = new SKPoint(_location.X, clientRectangle.Top);
            else if (rect.Bottom > clientRectangle.Bottom)
                _location = new SKPoint(_location.X, clientRectangle.Bottom - rect.Height);
        }
        private void DrawMinimizeButton(SKCanvas canvas)
        {
            float minHalfLength = MinimizeButton.Width / 4;
            if (Minimized)
            {
                canvas.DrawLine(MinimizeButton.MidX - minHalfLength,
                    MinimizeButton.MidY,
                    MinimizeButton.MidX + minHalfLength,
                    MinimizeButton.MidY,
                    SymbolPaint);
                canvas.DrawLine(MinimizeButton.MidX,
                    MinimizeButton.MidY - minHalfLength,
                    MinimizeButton.MidX,
                    MinimizeButton.MidY + minHalfLength,
                    SymbolPaint);
            }
            else
                canvas.DrawLine(MinimizeButton.MidX - minHalfLength,
                    MinimizeButton.MidY,
                    MinimizeButton.MidX + minHalfLength,
                    MinimizeButton.MidY,
                    SymbolPaint);
        }

        private void InitializeResizeTriangle()
        {
            float triangleSize = 10.5f * ScaleFactor;
            var bottomRight = new SKPoint(Rectangle.Right, Rectangle.Bottom);
            var topOfTriangle = new SKPoint(bottomRight.X, bottomRight.Y - triangleSize);
            var leftOfTriangle = new SKPoint(bottomRight.X - triangleSize, bottomRight.Y);

            var path = new SKPath();
            path.MoveTo(bottomRight);
            path.LineTo(topOfTriangle);
            path.LineTo(leftOfTriangle);
            path.Close();
            var old = Interlocked.Exchange(ref _resizeTriangle, path);
            old?.Dispose();
        }

        private void DrawResizeCorner(SKCanvas canvas)
        {
            var path = ResizeTriangle;
            if (path is not null)
                canvas.DrawPath(path, TitleBarPaint);
        }
        private WidgetClickEvent HitTest(SKPoint point)
        {
            var result = WidgetClickEvent.None;
            bool clicked = point.X >= Rectangle.Left && point.X <= Rectangle.Right && point.Y >= Rectangle.Top && point.Y <= Rectangle.Bottom;
            if (!clicked)
                return result;
            result = WidgetClickEvent.Clicked;
            bool titleClicked = point.X >= TitleBar.Left && point.X <= TitleBar.Right && point.Y >= TitleBar.Top && point.Y <= TitleBar.Bottom;
            if (titleClicked)
                result = WidgetClickEvent.ClickedTitleBar;
            bool clientClicked = point.X >= ClientRectangle.Left && point.X <= ClientRectangle.Right && point.Y >= ClientRectangle.Top && point.Y <= ClientRectangle.Bottom;
            if (!Minimized && clientClicked)
                result = WidgetClickEvent.ClickedClientArea;
            bool minClicked = point.X >= MinimizeButton.Left && point.X <= MinimizeButton.Right && point.Y >= MinimizeButton.Top && point.Y <= MinimizeButton.Bottom;
            if (minClicked)
                result = WidgetClickEvent.ClickedMinimize;
            var resizePath = _resizeTriangle;
            if (!Minimized && resizePath is not null && resizePath.Contains(point.X, point.Y))
                result = WidgetClickEvent.ClickedResize;
            return result;
        }
        #endregion

        #region Paints
        private static readonly SKPaint WidgetBackgroundPaint = new SKPaint()
        {
            Color = SKColors.Black.WithAlpha(0xBE),
            StrokeWidth = 1,
            Style = SKPaintStyle.Fill,
        };

        private static readonly SKPaint TitleBarPaint = new SKPaint()
        {
            Color = SKColors.Gray,
            StrokeWidth = 0.5f,
            Style = SKPaintStyle.Fill,
        };

        private static readonly SKPaint ButtonBackgroundPaint = new SKPaint()
        {
            Color = SKColors.LightGray,
            StrokeWidth = 0.1f,
            Style = SKPaintStyle.Fill,
        };

        private static readonly SKPaint SymbolPaint = new SKPaint()
        {
            Color = SKColors.Black,
            StrokeWidth = 2f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true
        };

        private static readonly SKPaint TitleBarText = new SKPaint()
        {
            SubpixelText = true,
            Color = SKColors.White,
            IsStroke = false,
            TextSize = 9f,
            TextAlign = SKTextAlign.Left,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyRegular,
            FilterQuality = SKFilterQuality.High
        };
        #endregion

        #region IDisposable
        private bool _disposed = false;
        public virtual void Dispose()
        {
            bool disposed = Interlocked.Exchange(ref _disposed, true);
            if (!disposed)
            {
                _parent.MouseClick -= Parent_MouseClick;
                _parent.MouseDown -= Parent_MouseDown;
                _parent.MouseUp -= Parent_MouseUp;
                _parent.MouseMove -= Parent_MouseMove;
                _parent.Resize -= Parent_Resize;
                ResizeTriangle?.Dispose();
            }
        }
        #endregion
    }
}
