﻿using System.Windows;

namespace Ahed_project.MasterData
{
    public class ContentState
    {
        public Visibility LockVisibillity { get; set; } //виден ли замочек

        private bool isEnabled;
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                if (isEnabled == true)
                {
                    LockVisibillity = Visibility.Hidden;

                }
                else
                {
                    LockVisibillity = Visibility.Visible;
                }

            }
        }
    }
}
