import { RunComponentStatus } from "./runComponentStatus";

export  class RunDetail {
    RunDetailId: number;
    RunNumber: string;
    ApplicationId: number;
    RunNumberStatusId:number;
    Status: boolean;
    RunComponentStatus:RunComponentStatus[];
}
