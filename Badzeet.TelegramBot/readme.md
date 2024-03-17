# Telegram bot

Telegram Bot for each conversation in 3 states

## States 
- Unauthorized   
- Initialized  
- PaymentDrafted

## Commands for each state
### Unauthorized
- `help`
- `auth [code]` should register device with user account. user may have multiple devices

### Initialized
- `help`
- `report` - sends current budget summary
- `summary` - sends current budget summary
- `add [amount] [desctiption] [category]` - adds new payment

### PaymentDrafted
- `help`
- `cancel` - gets back to Initialized state

