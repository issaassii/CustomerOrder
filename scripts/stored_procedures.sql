CREATE OR REPLACE FUNCTION "GetCustomerOrderSummary"(p_customer_id INT)
RETURNS TABLE (
    "CustomerId" INT,
    "TotalOrders" BIGINT,
    "TotalSpent" NUMERIC
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        o."CustomerId",
        COUNT(o."Id") AS "TotalOrders",
        COALESCE(SUM(o."TotalAmount"), 0) AS "TotalSpent"
    FROM "Orders" o
    WHERE o."CustomerId" = p_customer_id
      AND o."IsDeleted" = false
    GROUP BY o."CustomerId";
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION "SearchOrders"(
    p_customer_id INT DEFAULT NULL,
    p_start_date TIMESTAMPTZ DEFAULT NULL,
    p_end_date TIMESTAMPTZ DEFAULT NULL
)
RETURNS TABLE (
    "Id" INT,
    "CustomerId" INT,
    "OrderDate" TIMESTAMPTZ,
    "TotalAmount" NUMERIC
) AS $$
BEGIN
    RETURN QUERY
    SELECT o."Id", o."CustomerId", o."OrderDate", o."TotalAmount"
    FROM "Orders" o
    WHERE o."IsDeleted" = false
      AND (p_customer_id IS NULL OR o."CustomerId" = p_customer_id)
      AND (p_start_date IS NULL OR o."OrderDate" >= p_start_date)
      AND (p_end_date IS NULL OR o."OrderDate" <= p_end_date)
    ORDER BY o."OrderDate" DESC;
END;
$$ LANGUAGE plpgsql;