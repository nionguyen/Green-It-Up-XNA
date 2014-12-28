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
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using System.Threading;
#endregion

namespace WindowsPhoneGame2
{
    class MainScreen : GameScreen
    {
        Texture2D backgroundTexture;
        Texture2D helpTexture;

        Texture2D musicOff;
        Texture2D gameWin;
        Texture2D gameLose;
        Texture2D cellGreen;
        Texture2D cellDirty;
        SpriteFont spriteFont;

        bool m_isHelpClicked;
        bool m_isWin;
        bool m_isMusicOn = true;

        int[,] m_cellArray;
        int boardSize;
        int cellSize;
        int m_gameState = (int)GameState.GameOn;

        Rectangle fullscreen;
        Rectangle popup = new Rectangle(215, 134, 370, 212);

        Rectangle musicButton = new Rectangle(41, 410, 58, 58);

        Rectangle cellRect;
        int m_stepCount = 0;
        int m_maxStep;
        float m_timeCount = 0;

        private GestureSample Gestures;//++++++++++++++++++++++++++++++++



        public MainScreen()
        {
            SoundManager.PlaySong(ESong.Background);//++++++++++++++++++++++++++++
            if (Global.gameMode == GameMode.Mode_3)
            {
                boardSize = 3;
                cellSize = 130;
                m_maxStep = 15;
            }
            else if (Global.gameMode == GameMode.Mode_5)
            {
                boardSize = 5;
                cellSize = 80;
                m_maxStep = 30;
            }
            else if (Global.gameMode == GameMode.Mode_7)
            {
                boardSize = 7;
                cellSize = 58;
                m_maxStep = 50;
            }




            m_cellArray = new int[boardSize, boardSize];

            for (int i = 0; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                    m_cellArray[i, j] = -1;

            int origin = (boardSize - 1) / 2;


            m_cellArray[origin - 1, origin] = 1;
            m_cellArray[origin, origin - 1] = 1;
            m_cellArray[origin, origin] = 1;
            m_cellArray[origin, origin + 1] = 1;
            m_cellArray[origin + 1, origin] = 1;

        }

        public override void LoadContent()
        {
            base.LoadContent();
            spriteFont = this.screenContent.Load<SpriteFont>("ScoreFont");
            backgroundTexture = screenContent.Load<Texture2D>("Resource/BG_Game");
            helpTexture = screenContent.Load<Texture2D>("Resource/PU_Help");

            musicOff = screenContent.Load<Texture2D>("Resource/SBT_MusicOff");

            gameWin = screenContent.Load<Texture2D>("Resource/PU_You_Win");
            gameLose = screenContent.Load<Texture2D>("Resource/PU_You_Lose");

            cellDirty = screenContent.Load<Texture2D>("Resource/G_Cell_Dirty");
            cellGreen = screenContent.Load<Texture2D>("Resource/G_Cell_Green");



        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }




        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            if (Global.Back == true)
            {
                SoundManager.PlaySound(ESound.SelectButton);
                ModeScreen modeScreen = new ModeScreen();
                ScreenManager.AddScreen(modeScreen);
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

                Rectangle replayButton = new Rectangle(637, 410, 58, 58);
                Rectangle menuButton = new Rectangle(105, 410, 58, 58);
                Rectangle helpButton = new Rectangle(701, 410, 58, 58);
                Rectangle boardArea = new Rectangle(190, 30, 420, 420);

                
                Point posTap = new Point((int)Gestures.Position.X, (int)Gestures.Position.Y);

               
                ///Click help button
                if (helpButton.Contains(posTap))
                {
                    SoundManager.PlaySound(ESound.SelectButton);//++++++++++++++++++++

                    m_isHelpClicked = !m_isHelpClicked;
                    if (m_isHelpClicked)
                        m_gameState = (int)GameState.GamePause;
                    else
                        m_gameState = (int)GameState.GameOn;
                    return;
                }

                //Click on the help pop up
                if (m_isHelpClicked)
                {
                    SoundManager.PlaySound(ESound.SelectButton);//++++++++++++++++++++
                    m_isHelpClicked = false;
                    m_gameState = (int)GameState.GameOn;
                    return;
                }

                /// Click on result pop up
                if (m_gameState == (int)GameState.GameEnd && popup.Contains(posTap))
                {
                    
                    SoundManager.PlaySound(ESound.SelectButton);//+++++++++++++++++++++++

                    if (m_isWin == true)
                    {
                        ModeScreen modeScreen = new ModeScreen();
                        ScreenManager.AddScreen(modeScreen);
                        this.ExitScreen();

                        return;
                    }
                    ResetBoard();
                    return;
                }

                /// Click the replay button
                if (replayButton.Contains(posTap))
                {
                    SoundManager.PlaySound(ESound.SelectButton);//+++++++++++++++++++++++
                    ResetBoard();
                    return;
                }

                /// Click the Menu button
                if (menuButton.Contains(posTap))
                {
                    SoundManager.PlaySound(ESound.SelectButton);//++++++++++++++++++++++
                    MenuScreen menuScreen = new MenuScreen();
                    ScreenManager.AddScreen(menuScreen);
                    this.ExitScreen();
                    return;
                }

                /// Click the music button
                if (musicButton.Contains(posTap))
                {
                    SoundManager.PlaySound(ESound.SelectButton);//++++++++++++++++++++
                    if (Global.SOUND == true)
                    {
                        Global.SOUND = false;
                        SoundManager.StopSongs();
                    }
                    else
                    {
                        Global.SOUND = true;
                        SoundManager.PlaySong(ESong.Background);
                    }
                    //++++++++++++++++++++++
                    m_isMusicOn = !m_isMusicOn;
                    return;
                }

                /// Click the game board
                if (boardArea.Contains(posTap) && m_gameState == (int)GameState.GameOn)
                {

                    CellChange(posTap);
                    checkResult();

                    return;

                }

            }

            
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            ////////////////////////////////////////////////////////////////
            spriteBatch.Begin();

            // Draw background
            spriteBatch.Draw(backgroundTexture, fullscreen, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));


            // Draw the board
            DrawCellArray(spriteBatch);

            // Draw the Step count
            DrawStep(spriteBatch);

            // Draw help pop up
            if (m_isHelpClicked)
                spriteBatch.Draw(helpTexture, popup, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            // Draw the result
            if (m_gameState == (int)GameState.GameEnd)
            {
                if (m_isWin)
                {
                    spriteBatch.Draw(gameWin, popup, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
                }
                else
                {
                    spriteBatch.Draw(gameLose, popup, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
                }
            }

            // Draw music button
            if (Global.SOUND == false)
                spriteBatch.Draw(musicOff, musicButton, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            /////////////////////////////////////////////////////////////////
            spriteBatch.End();
        }

        public void CellChange(Point posTap)
        {
            SoundManager.PlaySound(ESound.Success);//+++++++++++++++++++++++++++++++
            m_stepCount++;

            Point temp = posTap;
            temp.X -= 190;
            temp.Y -= 30;

            int col = temp.X / (420 / boardSize);
            int row = temp.Y / (420 / boardSize);

            m_cellArray[col, row] *= -1;

            int colTemp, rowTemp;

            colTemp = col - 1;
            rowTemp = row;
            if (colTemp < boardSize && colTemp >= 0 && rowTemp < boardSize && rowTemp >= 0)
                m_cellArray[col - 1, row] *= -1;

            colTemp = col + 1;
            rowTemp = row;
            if (colTemp < boardSize && colTemp >= 0 && rowTemp < boardSize && rowTemp >= 0)
                m_cellArray[col + 1, row] *= -1;

            colTemp = col;
            rowTemp = row - 1;
            if (colTemp < boardSize && colTemp >= 0 && rowTemp < boardSize && rowTemp >= 0)
                m_cellArray[col, row - 1] *= -1;

            colTemp = col;
            rowTemp = row + 1;
            if (colTemp < boardSize && colTemp >= 0 && rowTemp < boardSize && rowTemp >= 0)
                m_cellArray[col, row + 1] *= -1;
        }


        public void DrawCellArray(SpriteBatch spriteBatch)
        {

            for (int i = 0; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                {
                    cellRect = new Rectangle(190 + i * 420 / boardSize, 30 + j * 420 / boardSize, cellSize, cellSize);
                    if (m_cellArray[i, j] == -1)
                    {
                        spriteBatch.Draw(cellDirty, cellRect, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
                    }
                    else
                    {
                        spriteBatch.Draw(cellGreen, cellRect, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
                    }
                }


        }

        public void checkResult()
        {
            if (m_stepCount >= m_maxStep)
            {
                m_isWin = false;
                m_gameState = (int)GameState.GameEnd;
                SoundManager.PlaySound(ESound.Lose);
                return;
            }

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (m_cellArray[i, j] == -1)
                    {
                        m_isWin = false;
                        return;
                    }
                }


            }

            m_isWin = true;
            SoundManager.PlaySound(ESound.Win);
            m_gameState = (int)GameState.GameEnd;
            return;

        }
        public void DrawStep(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(spriteFont, (m_maxStep - m_stepCount).ToString(), new Vector2(70, 80), Color.White);
        }


        public void ResetBoard()
        {
            for (int i = 0; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                    m_cellArray[i, j] = -1;

            int origin = (boardSize - 1) / 2;


            m_cellArray[origin - 1, origin] = 1;
            m_cellArray[origin, origin - 1] = 1;
            m_cellArray[origin, origin] = 1;
            m_cellArray[origin, origin + 1] = 1;
            m_cellArray[origin + 1, origin] = 1;
            m_stepCount = 0;
            m_gameState = (int)GameState.GameOn;
        }
    }
}
