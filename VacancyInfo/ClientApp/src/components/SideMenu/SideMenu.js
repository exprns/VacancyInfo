import React, { useEffect, useState } from 'react';
import { Accordion, AccordionSummary, AccordionDetails, Typography, Button, Icon } from '@material-ui/core';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import AddIcon from '@material-ui/icons/Add';
import './SideMenu.css';

export const SideMenu = ({ titles }) => {
    const [isLoading, setLoading] = useState(false);
    const [regions, setRegions] = useState([]);

    useEffect(() => {
        setLoading(true);
        fetch(`api/Vacancy/GetAreasJson`)
            .then(res => res.json())
            .then(res => {
                console.log(res);
                setRegions(res);
            })
            .catch(err => console.error(err))
            .finally(() => setLoading(false));
    }, []);

    return <div>
        <Accordion className={ 'sideMenu' } disabled={ isLoading }>
            <AccordionSummary
                expandIcon={ <ExpandMoreIcon /> }
                aria-controls="panel1a-content"
                id="panel1a-header"
            >
                <Typography>Регион</Typography>
            </AccordionSummary>
            <AccordionDetails>
                <div className="column">
                    { regions.map(regions => (
                        <Button>
                            <div className={ "rowRegion" }>
                                <Typography>
                                    { regions.name }
                                </Typography>
                                <AddIcon />
                            </div>
                        </Button>
                    ))
                    }
                </div>
            </AccordionDetails>
        </Accordion>
    </div>
}