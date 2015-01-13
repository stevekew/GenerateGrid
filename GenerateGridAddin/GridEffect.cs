using System;
using Pinta;
using Pinta.Core;
using Pinta.Gui.Widgets;
using Mono.Addins;

namespace GenerateGridAddin
{
	public class GridEffect : Pinta.Core.BaseEffect
	{
		public GridEffect ()
		{
			EffectData = new GridEffectData ();
		}

		public override string Name { get {	return AddinManager.CurrentLocalizer.GetString ("Generate Grid Title"); } }

		public override bool IsConfigurable { get { return true; } }

		public override bool LaunchConfiguration ()
		{
			// Return true if the user accepted the configuration.
			// Return false if the user cancelled.
			return EffectHelper. LaunchSimpleEffectDialog (this, AddinManager.CurrentLocalizer);
		}

		// The render algorithm is from BoltBait's Grid Maker v3.0 plugin for Paint.net.
		// http://forums.getpaint.net/index.php?/topic/1964-grid-maker-plugin-v30-updated-july-2-2014/
		public unsafe override void Render (Cairo.ImageSurface src, Cairo.ImageSurface dst, Gdk.Rectangle[] rects)
		{
			GridEffectData ged = EffectData as GridEffectData;

			int gridSize = ged.GridSize;
			int lineWidth = ged.LineWidth;
			bool checkBoard = ged.CheckerBoard;
			bool useCanvasForSecColor = ged.UseCanvasForSecondaryColor;
			ColorBgra primaryColor = PintaCore.Palette.PrimaryColor.ToColorBgra ();
			ColorBgra secondaryColor = PintaCore.Palette.SecondaryColor.ToColorBgra ();


			bool Odd = false;

			foreach (Gdk.Rectangle rect in rects) {
				for (int y = rect.Top; y <= rect.Bottom; y++) {

					ColorBgra* srcPtr = src.GetPointAddressUnchecked (rect.Left, y);
					ColorBgra* dstPtr = dst.GetPointAddressUnchecked (rect.Left, y);
					for (int x = rect.Left; x <= rect.Right; x++) {
						ColorBgra CurrentPixel = *srcPtr;

						if (checkBoard) {
							// We're making a checker board
							Odd = true;
							if (((x / gridSize) % 2) == 0)
								Odd = false;
							if (((y / gridSize) % 2) == 0)
								Odd = !Odd;
							if (Odd) {
								CurrentPixel = primaryColor;
							} else {
								if (!useCanvasForSecColor) {
									CurrentPixel = secondaryColor;
								}
							}
						} else {
							// We're making a grid
							Odd = false;
							for (int t = 0; t < lineWidth; t++) {
								if ((((x - t) % gridSize) == 0) || (((y - t) % gridSize) == 0))
									Odd = true;
								if (Odd) {
									CurrentPixel = primaryColor;
								} else {
									if (!useCanvasForSecColor) {
										CurrentPixel = secondaryColor;
									}
								}
							}
						}

						*dstPtr = CurrentPixel;
						srcPtr++;
						dstPtr++;
					}
				}
			}
		}

		public class GridEffectData : EffectData
		{
			[MinimumValue(0), MaximumValue(500), Caption("Grid Size")]
			public int GridSize = 10;

			[MinimumValue(0), MaximumValue(100), Caption("Line Width")]
			public int LineWidth = 1;

			[Caption("Checkerboard")]
			public bool CheckerBoard = false;

			[Caption("Use canvas for secondary color")]
			public bool UseCanvasForSecondaryColor = true;
		}
	}
}

