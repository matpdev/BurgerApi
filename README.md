# BURGER API

Uma api criada para serve de base para um sistema básico de fast food, com pagamento, leitura e verificação de JWT e Webhook.

### Funcionalidades:
- Pagamentos
    - Utilizando sistema do Stripe para efetuar os pagamentos.
- Webhooks
- OpenAPI/Swagger
- JWT
- Envio e confirmação de Email

### Estado Atual: <b>Pre-Alpha</b>

Arquitetura utilizada: MVC + Arquitetura Limpa

## Padrão de Pastas:
- App ->
- Models ->
    - Entities
- Utils
    - Cipher Encrypt
- Controllers
    - Dto
- Identity
- Middlewares
- Services
- Migrations

## Atividades

- [ ] - Rotas do Produto
    - [ ] - Adicionar
    - [ ] - Editar
    - [ ] - Excluir
    - [ ] - Buscar Todos
- [ ] - Rotas de Usuário
    - [x] - Login
    - [x] - Registro
    - [ ] - Edição
    - [x] - Confirmação de Email
        - -> Confirmação do Email irá utilizar o SMTP da Google, com um link que ligará até a api com um código temporário ligado ao usuário ao queal recebeu esse email. Deverá conter uma query com um código com possibilidade de invalidar. Utilizar UUID para a criação do código, junto aos dados do usuário que serão utilizados.
- [ ] - Rotas de Pagamento
    - [ ] - Criar uma ordem
    - [ ] - Editar ordem
    - [ ] - Mover para finalizado/entregue
    - [ ] - Apagar ordem
- [ ] - Webhook para o Stripe
    - [ ] - Verificar e Atualizar a ordem
    - [ ] - Criar um id para o vínculo do usuário
