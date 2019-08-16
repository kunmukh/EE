view wave 
wave clipboard store
wave create -driver freeze -pattern clock -initialvalue HiZ -period 25ps -dutycycle 50 -starttime 0ps -endtime 1000ps sim:/GPRegister/clk 
wave create -driver freeze -pattern clock -initialvalue HiZ -period 500ps -dutycycle 50 -starttime 0ps -endtime 1000ps sim:/GPRegister/D 
wave create -driver freeze -pattern constant -value 1010 -range 3 0 -starttime 0ps -endtime 1000ps sim:/GPRegister/BusIn 
WaveExpandAll -1
wave create -driver freeze -pattern clock -initialvalue 0 -period 500ps -dutycycle 50 -starttime 0ps -endtime 1000ps sim:/GPRegister/L 
WaveCollapseAll -1
wave clipboard restore
