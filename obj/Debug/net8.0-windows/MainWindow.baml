   M S B A M L   `   `   `  ����  BlackJackApp    BlackjackApp.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  	Blackjack��5   	   $	��550��6   $	��800��6(   
��� d �.��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   ��  �� Orientation$ Vertical=�6   $
��Left=�6,   $	��Top=�6G   $��10q�6_   $��0��6k   .+�5      �� -   5         ��Name$ PlayerBalance��5      $��16(�6/   �� 5      -   5        $ PlayerStatus��5      $��16(�6.   �� 5      -   5         ��Name$ 
PlayerHand��5      $ 
Horizontal=�6-   �� 5       -   5        $ 	PotAmount��5       $��16(�6+   $��10q�69   $��Stretch=�6E   �� 5"      -   5        $ DealerBalance��5"      $��16(�6/   �� 5$      -   5        $ DealerStatus��5$      $��16(�6.   �� 5&      -   5        $ 
DealerHand��5&      $ 
Horizontal=�6-   5(      �� 5+   
   $ 
Horizontal=�6   $��Stretch=�6.   $��Bottom=�6L   $��2��6g   $��10q�6t   .+�5-      �� -   5        $��Взять карту��5-      $	��100��6+   $��5q�67   6B   �� 5/      -	   5        $��Стоп��5/      $	��100��6$   $��5q�60   6;   �� 51      -
   5        $��Новая игра��51      $	��100��6*   $��5q�66   6A   53      55      57      