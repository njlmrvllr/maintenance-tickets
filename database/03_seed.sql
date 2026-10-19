-- Maintenance Tickets - sample data for local development.
-- Uses the stored procedures so every ticket gets a consistent history.
-- Expects an empty database: ticket ids 1 to 5 are created in this order.

CALL sp_create_ticket('ATM out of service', 'ATM-0451', 'The screen does not respond and the card slot is blocked.', 'Carlos Ruiz');
CALL sp_create_ticket('Water leak on second floor', 'SEDE-LIM', 'Water is dripping from the ceiling next to the meeting room.', 'Ana Quispe');
CALL sp_create_ticket('UPS alarm triggered', 'SEDE-ARE', 'The UPS in the server room beeps continuously.', 'Luis Medina');
CALL sp_create_ticket('Card reader replacement', 'ATM-0210', 'The card reader rejects valid cards intermittently.', 'Rosa Ching');
CALL sp_create_ticket('Air conditioning failure', 'SEDE-LIM', 'The air conditioning in the main hall is not cooling.', 'Jorge Salas');

-- Ticket 3: in progress
CALL sp_change_ticket_status(3, 'PENDING', 'IN_PROGRESS', NULL, 'Technician assigned to check the UPS.', 'Lucia Mendez');

-- Ticket 4: resolved
CALL sp_change_ticket_status(4, 'PENDING', 'IN_PROGRESS', NULL, 'Replacement part requested.', 'Lucia Mendez');
CALL sp_change_ticket_status(4, 'IN_PROGRESS', 'RESOLVED', 'Faulty card reader replaced with a new unit.', 'Card reader replaced and tested.', 'Lucia Mendez');

-- Ticket 5: resolved and closed
CALL sp_change_ticket_status(5, 'PENDING', 'IN_PROGRESS', NULL, 'HVAC technician scheduled.', 'Diego Paredes');
CALL sp_change_ticket_status(5, 'IN_PROGRESS', 'RESOLVED', 'Refrigerant recharged and filter cleaned.', 'Cooling restored.', 'Diego Paredes');
CALL sp_change_ticket_status(5, 'RESOLVED', 'CLOSED', NULL, 'Confirmed by the branch manager.', 'Diego Paredes');
