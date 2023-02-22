﻿using Sandbox;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Utils;
using VRageMath;

namespace avaness.PluginLoader.GUI
{
    public abstract class PluginScreen : MyGuiScreenBase
    {
        protected const float GuiSpacing = 0.0175f;

        public PluginScreen(Vector2? position = null, Vector2? size = null) : 
            base(position ?? new Vector2(0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, size ?? new Vector2(0.5f), 
                backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
        {
            EnabledBackgroundFade = true;
            m_closeOnEsc = true;
            m_drawEvenWithoutFocus = true;
            CanHideOthers = true;
            CanBeHidden = true;
            CloseButtonEnabled = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            RecreateControls(true);
        }

        protected MyLayoutTable GetLayoutTableBetween(MyGuiControlBase top, MyGuiControlBase bottom, float verticalSpacing = GuiSpacing, float horizontalSpacing = GuiSpacing)
        {
            Vector2 halfSize = m_size.Value / 2;

            float topPosY = GetCoordTopLeftFromAligned(top).Y;
            Vector2 topPos = new Vector2(horizontalSpacing - halfSize.X, topPosY + top.Size.Y + verticalSpacing) ;

            float bottomPosY = GetCoordTopLeftFromAligned(bottom).Y;
            Vector2 bottomPos = new Vector2(halfSize.X - horizontalSpacing, bottomPosY - verticalSpacing);

            Vector2 size = bottomPos - topPos;
            size.X = Math.Abs(size.X);
            size.Y = Math.Abs(size.Y);
            
            return new MyLayoutTable(this, topPos, size);
        }

        protected Vector2 GetTopLeftAfter(MyGuiControlBase control, float spacing = GuiSpacing)
        {
            Vector2 controlPos = control.GetPositionAbsoluteBottomLeft();
            Vector2 halfSize = m_size.Value / 2;
            return new Vector2(-halfSize.X, (controlPos.Y - halfSize.Y) + spacing);
        }

        protected Vector2 GetTopLeftBefore(MyGuiControlBase control, float spacing = GuiSpacing)
        {
            Vector2 controlPos = control.GetPositionAbsoluteBottomLeft();
            Vector2 halfSize = m_size.Value / 2;
            return new Vector2(-halfSize.X, (controlPos.Y - halfSize.Y) + spacing);
        }

        protected void AddBarBelow(MyGuiControlBase control, float barWidth = 0.8f, float spacing = GuiSpacing)
        {
            MyGuiControlSeparatorList bar = new MyGuiControlSeparatorList();
            barWidth *= m_size.Value.X;
            float controlTop = GetCoordTopLeftFromAligned(control).Y;
            bar.AddHorizontal(new Vector2(barWidth * -0.5f, controlTop + spacing + control.Size.Y), barWidth);
            Controls.Add(bar);
        }

        protected void AddBarAbove(MyGuiControlBase control, float barWidth = 0.8f, float spacing = GuiSpacing)
        {
            MyGuiControlSeparatorList bar = new MyGuiControlSeparatorList();
            barWidth *= m_size.Value.X;
            float controlTop = GetCoordTopLeftFromAligned(control).Y;
            bar.AddHorizontal(new Vector2(barWidth * -0.5f, controlTop - spacing), barWidth);
            Controls.Add(bar);
        }

        protected void AdvanceLayout(ref MyLayoutVertical layout, float amount)
        {
            layout.Advance(amount * MyGuiConstants.GUI_OPTIMAL_SIZE.Y);
        }

        protected void AdvanceLayout(ref MyLayoutHorizontal layout, float amount)
        {
            layout.Advance(amount * MyGuiConstants.GUI_OPTIMAL_SIZE.Y);
        }

        protected Vector2 GetCoordTopLeftFromAligned(MyGuiControlBase control)
        {
            return MyUtils.GetCoordTopLeftFromAligned(control.Position, control.Size, control.OriginAlign);
        }

        /// <summary>
        /// Positions <paramref name="newControl"/> to the right of <paramref name="currentControl"/> with a spacing of <paramref name="spacing"/>.
        /// </summary>
        protected void PositionToRight(MyGuiControlBase currentControl, MyGuiControlBase newControl, MyAlignV align = MyAlignV.Center, float spacing = GuiSpacing)
        {
            Vector2 currentTopLeft = GetCoordTopLeftFromAligned(currentControl);
            currentTopLeft.X += currentControl.Size.X + spacing;
            switch (align)
            {
                case MyAlignV.Top:
                    newControl.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
                    break;
                case MyAlignV.Center:
                    currentTopLeft.Y += currentControl.Size.Y / 2;
                    newControl.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
                    break;
                case MyAlignV.Bottom:
                    currentTopLeft.Y += currentControl.Size.Y;
                    newControl.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
                    break;
                default:
                    return;
            }
            newControl.Position = currentTopLeft;
        }

        /// <summary>
        /// Positions <paramref name="newControl"/> to the left of <paramref name="currentControl"/> with a spacing of <paramref name="spacing"/>.
        /// </summary>
        protected void PositionToLeft(MyGuiControlBase currentControl, MyGuiControlBase newControl, MyAlignV align = MyAlignV.Center, float spacing = GuiSpacing)
        {
            Vector2 currentTopLeft = GetCoordTopLeftFromAligned(currentControl);
            currentTopLeft.X -= spacing;
            switch (align)
            {
                case MyAlignV.Top:
                    newControl.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
                    break;
                case MyAlignV.Center:
                    currentTopLeft.Y += currentControl.Size.Y / 2;
                    newControl.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
                    break;
                case MyAlignV.Bottom:
                    currentTopLeft.Y += currentControl.Size.Y;
                    newControl.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
                    break;
                default:
                    return;
            }
            newControl.Position = currentTopLeft;
        }

        protected void AddImageToButton(MyGuiControlButton button, string iconTexture, float iconSize = 1)
        {
            MyGuiControlImage icon = new MyGuiControlImage(size: button.Size * iconSize, textures: new[] { iconTexture });
            button.HighlightChanged += delegate (MyGuiControlBase control) { icon.ColorMask = (control.HasHighlight ? MyGuiConstants.HIGHLIGHT_TEXT_COLOR : Vector4.One); };
            button.Elements.Add(icon);
        }
    }
}
