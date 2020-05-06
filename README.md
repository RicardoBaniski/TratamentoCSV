AULA 6 – EXERCÍCIO DE ETL

Instruções:

1 - Baixe os arquivos do repositório abaixo e salve em C:\TEMP
https://github.com/CSSEGISandData/COVID-19/tree/master/csse_covid_19_data/csse_covid_19_daily_reports

2 - Execute os Scripts da pasta: \TratamentoCSV\Scripts no SQL Server

3 - Ajuste a connectString abaixo conforme as configurações do seu Banco de Dados onde executou os Scripts:
SqlConnection(@"Data Source=AVELL\SQLEXPRESS;Initial Catalog=covid;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
