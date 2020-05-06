**AULA 6 – EXERCÍCIO DE ETL**

*Instruções:

1 - Execute no no SQL Server o "Script_Completo" da pasta: "\TratamentoCSV\Scripts";

2 - Ajuste a connectString abaixo conforme as configurações do seu Banco de Dados onde executou os Scripts:
SqlConnection(@"Data Source=AVELL\SQLEXPRESS;Initial Catalog=covid;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

3 - Ao final da execução da aplicação, execute o "SCRIPT_PERGUNTAS" da pasta: "\TratamentoCSV\Scripts";

**OBSERVAÇÃO:** 
Para atualizar os arquivos diários do COVID, acesse o repositório:
"https://github.com/CSSEGISandData/COVID-19/tree/master/csse_covid_19_data/csse_covid_19_daily_reports", 
e salve os arquivos na pasta "\TratamentoCSV\csse_covid_19_daily_reports"
