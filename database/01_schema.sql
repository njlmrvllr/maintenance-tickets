-- Maintenance Tickets - database schema (MySQL 8.4)
-- One ticket has many history records (1 - N).

CREATE TABLE tickets (
    id           INT           NOT NULL AUTO_INCREMENT,
    title        VARCHAR(120)  NOT NULL,
    asset_code   VARCHAR(30)   NOT NULL,
    description  VARCHAR(1000) NOT NULL,
    status       VARCHAR(20)   NOT NULL DEFAULT 'PENDING',
    diagnosis    VARCHAR(1000) NULL,
    reported_by  VARCHAR(80)   NOT NULL,
    created_at   DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at   DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT pk_tickets PRIMARY KEY (id),
    CONSTRAINT ck_tickets_status
        CHECK (status IN ('PENDING', 'IN_PROGRESS', 'RESOLVED', 'CLOSED', 'CANCELLED'))
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

CREATE INDEX ix_tickets_status ON tickets (status);

CREATE TABLE ticket_history (
    id           INT          NOT NULL AUTO_INCREMENT,
    ticket_id    INT          NOT NULL,
    from_status  VARCHAR(20)  NULL,
    to_status    VARCHAR(20)  NOT NULL,
    comment      VARCHAR(500) NULL,
    changed_by   VARCHAR(80)  NOT NULL,
    changed_at   DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_ticket_history PRIMARY KEY (id),
    CONSTRAINT fk_ticket_history_tickets
        FOREIGN KEY (ticket_id) REFERENCES tickets (id)
        ON DELETE RESTRICT
        ON UPDATE CASCADE,
    CONSTRAINT ck_ticket_history_from_status
        CHECK (from_status IS NULL
               OR from_status IN ('PENDING', 'IN_PROGRESS', 'RESOLVED', 'CLOSED', 'CANCELLED')),
    CONSTRAINT ck_ticket_history_to_status
        CHECK (to_status IN ('PENDING', 'IN_PROGRESS', 'RESOLVED', 'CLOSED', 'CANCELLED'))
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

CREATE INDEX ix_ticket_history_ticket_id ON ticket_history (ticket_id);
