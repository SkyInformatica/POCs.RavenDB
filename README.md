# Pré-requisitos

## Executar o RavenDB
Antes de mais nada, é necessário ter uma instância do RavenDB rodando.
O projeto foi desenvolvido com o RavenDB sendo executado no Docker via
WSL 2. Recomendamos este uso pela praticidade e facilidade de, tendo
algo de errado ocorrido, começar do zero.

## Configurar o RavenDB
Após colocar uma instância do RavenDB para rodar, é necessário criar
um banco de dados novo - recomendamos chamá-lo de "loja", dado o 
exemplo utilizado. Não é necessário nenhum outro ajuste, as coleções
e documentos serão inseridos via API.

## Configurar o arquivo "appsettings.json"
Dentro do projeto há um arquivo chamado "appsettings.json". Nele, é
necessário preencher três campos:
* Rota para o banco de dados;
* Nome do banco de dados;
* Rota para a API .NET.

Esta última informação você pode pegar após executar o programa pela
primeira vez. O arquivo já vem com os campos preenchidos para servir
de orientação à formatação do dado.

# Execução do Programa
Após atender aos pré-requisitos, basta executar o programa e utilizá-lo
via Swagger, Postman ou qualquer interface de sua preferência. O Swagger
é o recomendado neste cenário, tendo em vista que ele realiza o mapeamento
dos endpoints e das entidades de forma automática.

# Utilização

## Executar verbo Post no endpoint "/api/manutencao-raven/popula-banco"
Embora não seja obrigatório, este endpoint auxilia nos testes com índice
do RavenDB para já pré-fabricar alguns documentos. São 16 clientes e 16
produtos - os produtos que cada cliente possui são aleatórios. Ou seja,
toda vez que o método for chamado, os produtos dos clientes mudarão.

## Criação de Índices
Por padrão, o programa vem com quatro índices já criados: Clientes_PorNomeSobrenome,
Clientes_Por Produtos, Produtos_PorDescrição e Produtos_PorValor. Eles são
inicializados no banco através da classe ValidadorIndice, a qual realiza
uma chamada à API ManutencaoRavenController, a fim de cadastrá-lo no
RavenDB.

Caso queira criar um índice personalizado, basta cadastrar a classe
no namespace adequado e utilizar ValidadorIndice para inicializá-lo.
Para compreender melhor, sugerimos ver a implementação disso no
endpoint de consulta de cliente por nome e sobrenome.