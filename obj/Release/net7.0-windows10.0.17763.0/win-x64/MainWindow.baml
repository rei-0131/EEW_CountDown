   M S B A M L   `   `   `  ���� J  FKyoshin_Rei_WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Kyoshin_Rei_WPFKyoshin_Rei_WPF       Kyoshin_Rei_WPF.MainWindow   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     /localclr-namespace:Kyoshin_Rei_WPF       � Title$  EEW CountDown��$	��201��$	��800�����	#FF1F2025.��� -   $	��ALL��$	��Top=�$	��184�� � RowDefinitions �� $	��20* ��� $
��133* ��� $	��31* �.+��� $��2��$��23��$	��Top=�.+�� $��0��$��	0,0,0,165q� �� $	��25* � � ColumnDefinitions �� $
��255* ��� $	��73* ��� $	��81* ��� $
��190* ��� $	��73* ��� $
��128* �.+��� -    ��Name$ 
EEW_Status��$
��Wrap=�$6��0緊急地震速報は発表されていません�� �� TextAlignment$ Center=� J� RenderTransformOrigin$ 0.283,-0.481&�$��14(�
�������$��0��$��2,0,1,1q��� -   $ 	EEW_Level��$
��Wrap=�$ Center=�$ 0.283,-0.481&�$��14(�
�������$��1���� -   $	 mag��$��1��$
��Wrap=�
�������$��14(�$ Center=�$��3��$��
71,0,189,0q��� -   $ region_point��$��2��$��	80,0,72,1q�$
��Wrap=�$ -0.331,-0.418&�
�������$��14(�$ Center=�$��3���� -   $ now_time��$��5��$��0,0,0,1q�$
��Wrap=�$ -0.331,-0.418&�
�������$��13(�$ Center=��� -   $ 
report_num��$��4��$
��Wrap=�
�������$��14(�$ Center=�$��1,0,0,0q�A� -    A�Name$ time_ref��$��5��$��0,13,0,1q�$��Right=�$��6��$ 0.5,0.5&�
������� J� Opacity$ 0Q�z�V� .~���  �� ScaleY$ -1Q��� �� T� � $	��133��$��1��$��1�� �� $
��133* ��� $
��110* ��� $
��170* ��� $��50 ��� $
��110* ��� $
��170* ��� $	��57* �.+��� -	   	 ��Name$	 max_intensity��$��0,0,3,1q��� -
   $ msg_1��$
��Wrap=�$��P波到着まで��$��26��$	��Top=�
�������$��20(�$ Center=�$ 0.521,2.621&�$��115,107,152,0q�$��3���� -   $ p_second��$��2��$
��Wrap=�$��--秒��
�������$��48(�$ Center=�$��0,62,0,0q��� -   $ mag_2��$��3��$
��Wrap=�$��S波到着まで��$��26��$	��Top=�
�������$��20(�$ Center=�$ 0.521,2.621&�$��32,107,152,0q�$��3���� -   $ s_second��$��5��$
��Wrap=�$��--秒��
�������$��48(�$ Center=�$��0,62,0,0q��� -   $	 refresh��$��
41,117,0,1q�$
��Left=�$��15��$��6��$-��'/Resources/refresh-removebg-preview.png��
 J� OpacityMask

 �   ��� -    ��Name$ refresh_button��$��6��$�� ��$��
41,117,0,0q���Z� � -    �Name$ 
s_progress��$��2��.+�� -    �Name$ s_progressbar��$��Center=�$	��800�����	#FFFF0C00$��0,0,0,-2q�