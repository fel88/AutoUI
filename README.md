#  AutoUI ![flask](https://user-images.githubusercontent.com/15663687/183991126-7f8362d9-a349-4de4-a426-680e07334c0a.png)

GUI automatic testing tool
<img width="1257" height="762" alt="image" src="https://github.com/user-attachments/assets/1f459746-9506-4e34-8188-2c7ad6640db9" />

## How to use:
1. Open AutoUI
2. Load notepad_test.axml sample (from Samples folder) (Environment->Load)
3. Press Run all

## How to use as part of CI/CD (with GitLab)
### GitLab Runner machine
1. Instal PostgreSQL
2. Create database (dotnet ef --project AutoUI.Queue database update)
3. Run AutoUI.Queue
4. Add step to GitLab pipline to call AutoUI.Client

### VMWare machine
1. Install OBS Studio (to record video of each test), and enable OBS webosocket plugin
2. Share folder between VMWare machine and host machine (machine with AutoUI.Queue)
3. Run AutoUI.Server

### Web page
1. Run any web server and show data from DB (e.g. HttpSandbox+Dapper+Tabulator)
   

<sub>Some icons by [Yusuke Kamiyamane](http://p.yusukekamiyamane.com/). Licensed under a [Creative Commons Attribution 3.0 License](http://creativecommons.org/licenses/by/3.0/)</sub>
