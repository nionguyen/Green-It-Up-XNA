#region File Description
//-----------------------------------------------------------------------------
// BackgroundScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Phone.Tasks;
using System.Windows;
#endregion

namespace WindowsPhoneGame2
{
    class ModeScreen : GameScreen
    {
        #region Fields

        Texture2D backgroundTexture;
        float m_timeCount = 0;

        private GestureSample Gestures;//++++++++++++++++++++++++++++++++


        #endregion

        #region Initialization

        public ModeScreen()
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
            backgroundTexture = screenContent.Load<Texture2D>("Resource/BG_Mode");
            if (MessageBox.Show("YOUR SMILE IS OUR GOAL. okay?", "Do you enjoy this game? Then rate 5 star to favor us", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
                marketplaceReviewTask.Show();
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }


        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            m_timeCount += gameTime.ElapsedGameTime.Milliseconds;


            
            if (Global.Back == true)
            {
                SoundManager.PlaySound(ESound.SelectButton);
                MenuScreen menuScreen = new MenuScreen();
                ScreenManager.AddScreen(menuScreen);
                this.ExitScreen();
                return;
            }
            if (TouchPanel.IsGestureAvailable)
            {
                Gestures = TouchPanel.ReadGesture();
            }
            else
            {
                Gestures = new GestureSample();
            }


            if (Gestures.GestureType == GestureType.Tap) ///+++++++++++++++++++++++++++++++++
            {
                m_timeCount = 0;

                Rectangle Three = new Rectangle(447, 49, 75, 75);
                Rectangle Five = new Rectangle(447, 154, 98, 98);
                Rectangle Seven = new Rectangle(447, 290, 134, 134);
                /// +++++++++++++++++++++++++++++++
                
                Microsoft.Xna.Framework.Point posTap = new Microsoft.Xna.Framework.Point((int)Gestures.Position.X, (int)Gestures.Position.Y);

                if (Three.Contains(posTap))
                {
                    SoundManager.PlaySound(ESound.SelectButton);//++++++++++++++++++++
                    Global.gameMode = GameMode.Mode_3;

                    MainScreen mainScreen = new MainScreen();
                    ScreenManager.AddScreen(mainScreen);
                    this.ExitScreen();
                    return;
                }

                if (Five.Contains(posTap))
                {
                    SoundManager.PlaySound(ESound.SelectButton);//++++++++++++++++++++
                    Global.gameMode = GameMode.Mode_5;
                    MainScreen mainScreen = new MainScreen();
                    ScreenManager.AddScreen(mainScreen);
                    this.ExitScreen();
                    return;
                }

                if (Seven.Contains(posTap))
                {
                    SoundManager.PlaySound(ESound.SelectButton);//++++++++++++++++++++
                    Global.gameMode = GameMode.Mode_7;
                    MainScreen mainScreen = new MainScreen();
                    ScreenManager.AddScreen(mainScreen);
                    this.ExitScreen();
                    return;
                }


            }
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundTexture, fullscreen,
                             new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            spriteBatch.End();
        }


        #endregion
    }
}