   M S B A M L   `   `   `  ����K  GCheatMusic(Try2), Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:CheatMusic_Try2_CheatMusic_Try2_  5   	       CheatMusic_Try2_.SongText   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:CheatMusic_Try2_     5   	   V RMaterialDesignThemes.Wpf, Version=2.5.1.1345, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   -   5          �Name$  
TextWindow��5      $	��400��5	   	   $	��250��6    � WindowStyle$
 None=�6"    � 
ResizeMode$ NoResize=�65   
���   � � 	Resources 5
         ScrollThumbs �� 
TargetType �� Property �� Value �� 
TargetType �Name	 ��Name
 �� CornerRadius �� Triggers P� Property P� Value �� 
TargetName ]�Name ]� Thumb n�Name ]� IncreaseRepeatButton ��Name J� Opacity ]� DecreaseRepeatButton P� 
SourceName  ButtonSelectBrush  	DarkBrush �� Angle  PrimaryHueMidBrush �� Color  ButtonTextClose �� TargetProperty �� 
TargetName9  3System.Windows.Media.Animation.EasingDoubleKeyFrame O� KeyTime O� Value�� 5   
   %�	  &	        '�� D    &	 �    &	 �    �� 5      " n�6*   .�5      ��  � �� 5      �� 5      " n�6*   .R�5      � $
 Grid��6#   .+�5   "   �� $��Stretch=�6,   $��Stretch=�6J   $
��Auto��6f   $
��Auto��6s   
������ 6�   �� 5   "   $	 
Rectangle1��6)   $
 10��6=   $��Stretch=�6O   $��Stretch=�6m   $
��Auto��6�   $
��Auto��6�   #��z��6�   5       5      P� 5   "    �@�Tag$ 
Horizontal��69   . �5   &   �� $ 
Rectangle1��6-    � ��$
 Auto��6V   �� 5   &   $ 
Rectangle1��6-    � ��$ 7��6W   5   #   5      5      5      �� 5       " ��6/   .�5!      ��  �@��IsFlicksEnabled .  6;   �� 5"       � ��
 �����6/   �� 5#       � ��
 �����6/   �� 5$       � ��$ 10��6*   �� 5%       � �� 5&      �� 5'      " ��6*   .R�5(      � $ GridRoot��6#   $��10��65   ��6@   Z� .+�5)   "   ]� $ 
PART_Track��6(   $��0��6<   ��. |�.   5*   &   n� 5+   *   $ Thumb��60   #��z��6?   #���   6i    5-   &   �� 5.   *   $ PageUp��67   $��ScrollBar.PageDownCommand��6G   $ 0Q�6k   |�.  6w    50   &   �� 51   *   $ PageDown��67   $��ScrollBar.PageUpCommand��6I   $ 0Q�6k   |�.  6w   53   #   54       56      P� 57   "   $ Thumb��6*    �@J�IsMouseOver . . �58   &   �� # �  6-   $ Thumb��6Y    � ��6l   59   #   P� 5:   "   $ Thumb��6*    �@n�
IsDragging . . �5;   &   �� # �  6-   $ Thumb��6Q    � ��6d   5<   #   P� 5>   "    � {� .  . �5?   &   �� $ Thumb��6-    � y�$ 	Collapsed=�6V   5@   #   P� 5A   "    �@��Orientation$ 
Horizontal=�6A   . �5B   &   �� $ GridRoot��6-    �@�LayoutTransform 5C   *   �� 5D   .   $	 -90Q�6>   5F   '   �� 5G   &   $ 
PART_Track��6-    �@�LayoutTransform 5H   *   �� 5I   .   $	 -90Q�6>   5K   '   �� 5L   &    � ��$
 Auto��6>   �� 5M   &    � ��$ 12��6?   �� 5N   &   $ Thumb��6-    �@�Tag$ 
Horizontal��6O   �� 5O   &   $ PageDown��6-    � ��$ ScrollBar.PageLeftCommand��6V   �� 5P   &   $ PageUp��6-    � ��$  ScrollBar.PageRightCommand��6T   5Q   #   5S      5U      5V      �� 5W      $ 	#FFD6511E��69   �� 5X      .��5Y      U� $ (FrameworkElement.Width)�60   $ 
TextWindow��6e   .\�5Z        $ 0��6+   $	 250Q�67     5[      $ 0:0:0.3��6+   $ 0Q�6=   5\      5]      5^       � Triggers 5a      :� 5b   
    :� RoutedEvent$ ButtonBase.Click��6    :� 
SourceName$ ButtonTextClose��66   .[�5c      �� #��[ 6   5d      .��5g      � -   5        $ 	Text_Grid��5g      .+�5h   
   �� -   5          ��Name$  	field_Top��5h      $��30��6(   $	��Top=�64   $��5q�6L   ���	#FF121212.+�5i      � .+�5j      �� -   5        ! ��Name$! song_Name_Top��5j      " �� Padding$" 10 0 0 0q�63   $	��190��6F   ���	#FFB0C4DE$
��Left=�6i   $��Center=�6�   $��CharacterEllipsis=�6�   �� 5k      -   5        # ��Name$# ButtonTextClose��5k      $��Center=�62   $��Right=�6M   ��6i   Z� ��6   Z� .��5l      '  !MaterialDesignThemes.Wpf.PackIcon  �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.5.1.1345, Culture=neutral, PublicKeyToken=null]]$   Kind$$ 	ArrowLeft=�6.   ��5m      �� 5n      % �� EndPoint$% 0.5,1&�62   & � MappingMode$& RelativeToBoundingBox=�6C   .��5o   "   � ' � Color$' 	#FFD69016��6/   � 5p   "   $' 	#FFD6511E��6/   ( � Offset$( 0.747Q�6A   � 5q   "   $' 	#FF9B330D��6/   $( 0.807Q�6A   5r      5t      5u      5v      5w      �� 5y   
   -   5        $  field_Bottom��5y      $	��240��6+   $	��350��67   $��Bottom=�6D   $��5q�6_   .+�5z      � .+�5{      �� -   5        ) ��Name$) songText_Bottom��5{      "* �� VerticalScrollBarVisibility$
* Auto=�67   $+ �� HorizontalScrollBarVisibility$+ Hidden=�6Z   , �� 
IsReadOnly, . $��Center=�6�   $��Center=�6�   $	��240��6�   $	��345��6�   ���	#FF121212���	#FF121212���	#FFB0C4DE$��5q�6+  5|      5}      5~      5      