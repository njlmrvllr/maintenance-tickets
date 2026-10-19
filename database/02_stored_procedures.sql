-- Maintenance Tickets - stored procedures (MySQL 8.4)
-- Business rules (allowed transitions, required diagnosis/reason) live in the domain layer.
-- These procedures guarantee atomicity: every status change and its history record
-- are saved in the same transaction, or not at all.

DELIMITER $$

-- Creates a ticket in PENDING status and records the creation in its history.
DROP PROCEDURE IF EXISTS sp_create_ticket $$
CREATE PROCEDURE sp_create_ticket(
    IN p_title        VARCHAR(120),
    IN p_asset_code   VARCHAR(30),
    IN p_description  VARCHAR(1000),
    IN p_reported_by  VARCHAR(80)
)
BEGIN
    DECLARE v_ticket_id INT;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;

    INSERT INTO tickets (title, asset_code, description, status, reported_by)
    VALUES (p_title, p_asset_code, p_description, 'PENDING', p_reported_by);

    SET v_ticket_id = LAST_INSERT_ID();

    INSERT INTO ticket_history (ticket_id, from_status, to_status, comment, changed_by)
    VALUES (v_ticket_id, NULL, 'PENDING', 'Ticket created', p_reported_by);

    COMMIT;

    SELECT id, title, asset_code, description, status, diagnosis, reported_by, created_at, updated_at
    FROM tickets
    WHERE id = v_ticket_id;
END $$

-- Lists tickets, newest first. Pass NULL to get every status.
DROP PROCEDURE IF EXISTS sp_get_tickets $$
CREATE PROCEDURE sp_get_tickets(
    IN p_status VARCHAR(20)
)
BEGIN
    SELECT id, title, asset_code, description, status, diagnosis, reported_by, created_at, updated_at
    FROM tickets
    WHERE p_status IS NULL OR status = p_status
    ORDER BY created_at DESC, id DESC;
END $$

-- Returns a single ticket, or an empty result when it does not exist.
DROP PROCEDURE IF EXISTS sp_get_ticket_by_id $$
CREATE PROCEDURE sp_get_ticket_by_id(
    IN p_ticket_id INT
)
BEGIN
    SELECT id, title, asset_code, description, status, diagnosis, reported_by, created_at, updated_at
    FROM tickets
    WHERE id = p_ticket_id;
END $$

-- Moves a ticket to a new status and records the change in its history.
-- p_expected_status is the status the application read before validating the transition.
-- If the ticket no longer has that status (someone else changed it in the meantime),
-- nothing is saved and error 50409 is raised so the API can answer 409 Conflict.
DROP PROCEDURE IF EXISTS sp_change_ticket_status $$
CREATE PROCEDURE sp_change_ticket_status(
    IN p_ticket_id        INT,
    IN p_expected_status  VARCHAR(20),
    IN p_new_status       VARCHAR(20),
    IN p_diagnosis        VARCHAR(1000),
    IN p_comment          VARCHAR(500),
    IN p_changed_by       VARCHAR(80)
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;

    UPDATE tickets
    SET status     = p_new_status,
        diagnosis  = COALESCE(p_diagnosis, diagnosis),
        updated_at = CURRENT_TIMESTAMP
    WHERE id = p_ticket_id
      AND status = p_expected_status;

    IF ROW_COUNT() = 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MYSQL_ERRNO = 50409,
                MESSAGE_TEXT = 'Ticket not found or its status was changed by another operation.';
    END IF;

    INSERT INTO ticket_history (ticket_id, from_status, to_status, comment, changed_by)
    VALUES (p_ticket_id, p_expected_status, p_new_status, p_comment, p_changed_by);

    COMMIT;

    SELECT id, title, asset_code, description, status, diagnosis, reported_by, created_at, updated_at
    FROM tickets
    WHERE id = p_ticket_id;
END $$

-- Returns the history of a ticket in chronological order.
DROP PROCEDURE IF EXISTS sp_get_ticket_history $$
CREATE PROCEDURE sp_get_ticket_history(
    IN p_ticket_id INT
)
BEGIN
    SELECT id, ticket_id, from_status, to_status, comment, changed_by, changed_at
    FROM ticket_history
    WHERE ticket_id = p_ticket_id
    ORDER BY changed_at, id;
END $$

DELIMITER ;
